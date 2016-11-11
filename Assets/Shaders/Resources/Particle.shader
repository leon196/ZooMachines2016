Shader "Unlit/Particle" {
	Properties {
		_MainTex ("Texture (RGB)", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_ColorLight ("Color Light", Color) = (1,1,1,1)
		_ColorShadow ("Color Shadow", Color) = (0.3,0.3,0.3,1)
		_Radius ("Radius", Float) = 0.1
		_VelocityStretch ("Velocity Stretch", Float) = 0
		_VertexTexture ("Vertex (RGB)", 2D) = "white" {}
		_VertexInitialTexture ("Vertex Initial (RGB)", 2D) = "white" {}
		_VelocityTexture ("Velocity (RGB)", 2D) = "white" {}
		_Scale ("Scale", Vector) = (1,1,1,1)
	}
	SubShader {
		Tags { "Queue"="AlphaTest" "RenderType"="Transparent" "IgnoreProjector"="True" }
		// Blend One OneMinusSrcAlpha
		Blend SrcAlpha OneMinusSrcAlpha 
		// AlphaToMask On
		ZTest Off ZWrite Off
		Cull Off
		LOD 100
		Pass {
			CGPROGRAM
			#pragma exclude_renderers gles
			#pragma vertex vert
			#pragma geometry geom
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "Utils.cginc"

			// Common parameters
			sampler2D _MainTex;
			sampler2D _VertexTexture;
			sampler2D _VertexInitialTexture;
			sampler2D _VelocityTexture;
			sampler2D _ElementTexture;
			sampler2D _EdgeTexture;
			sampler2D _ColorTexture;
			float4 _MainTex_ST;
			float4 _Color;
			float4 _ColorLight;
			float4 _ColorShadow;
			float _Radius;
			float _VelocityStretch;
			float _RespawnDelayIn;
			float _RespawnDelayOut;
			float _RespawnNoiseScale;
			float _RespawnCycle;
			float2 _ResolutionEdge;

			// Local Transform matrix
			float4x4 _MatrixWorld;

			float3 _Target;
			float _TargetRadius;
			float4 _Scale;
			float _Beat;

			struct VS_INPUT
			{
				float4 vertex : POSITION;
				float3 normal	: NORMAL;
				float4 color	: COLOR;
				float4 texcoord2 : TEXCOORD1;
			};

			struct GS_INPUT
			{
				float4 vertex : POSITION;
				float3 normal	: NORMAL;
				float4 color	: COLOR;
				float4 texcoord2 : TEXCOORD1;
			};

			struct FS_INPUT {
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float4 texcoord2 : TEXCOORD1;
			};

			GS_INPUT vert (VS_INPUT v)
			{
				GS_INPUT o = (GS_INPUT)0;
				float4 vertex = v.vertex; 
				vertex.xyz = tex2Dlod(_VertexTexture, float4(v.texcoord2.xy, 0, 0)).rgb;
				o.vertex = vertex;
				o.color = v.color;
				o.normal = v.normal.xyz;
				o.texcoord2 = v.texcoord2;
				return o;
			}

			[maxvertexcount(3)]
			void geom (point GS_INPUT tri[1], inout TriangleStream<FS_INPUT> triStream)
			{
				FS_INPUT pIn = (FS_INPUT)0;
				pIn.texcoord2 = tri[0].texcoord2;
				pIn.normal = tri[0].normal;
				pIn.color = tri[0].color;

				float4 e = float4(1,1,1,1) * 0.00001;
				float3 viewDir = normalize(WorldSpaceViewDir(tri[0].vertex));
				float4 velocity = tex2Dlod(_VelocityTexture, float4(tri[0].texcoord2.xy, 0, 0));
				float4 element = tex2Dlod(_ElementTexture, float4(tri[0].texcoord2.xy, 0, 0));
				float4 position = tex2Dlod(_VertexTexture, float4(tri[0].texcoord2.xy, 0, 0));
				// pIn.color = normalize(velocity + e) * 0.5 + 0.5;

				fixed4 original = tex2Dlod(_VertexInitialTexture, float4(tri[0].texcoord2.xy, 0, 0));
				float3 target = _Target - mul(unity_ObjectToWorld, original);
				float shouldReset = step(_TargetRadius, length(target));

				float4 vertex = mul(unity_ObjectToWorld, tri[0].vertex);
				float aspect = _ScreenParams.y / _ScreenParams.x;
				float radius = _Radius;

				radius *= min(1.0, length(position));

				// radius *= lerp(smoothstep(0.0, _RespawnDelayIn, element.r), 1.0, shouldReset);
				// radius *= lerp(1. - smoothstep(_RespawnDelayOut, 1.0, element.r), 1.0, shouldReset);

				float3 tangent = normalize(velocity + e);
				// float3 tangent = normalize(vertex);// - target);
				// float3 tangent = normalize(cross(normalize(velocity + e), pIn.normal));
				// float3 tangent = normalize(cross(normalize(vertex - target), pIn.normal)) * _Scale.x;
				float3 up = normalize(cross(tangent, pIn.normal));
				// float3 up = normalize(vertex - target);
				// float3 up = normalize(velocity + e) * _Scale.y;// * 4.;
				// float3 cameraFront = normalize(UNITY_MATRIX_IT_MV[2].xyz);

				pIn.color = _Color;
				// float2 edgeUV = float2(0,0);
				// edgeUV.x = fmod(element.r, _ResolutionEdge.x) / _ResolutionEdge.x;
				// edgeUV.y = floor(element.r / _ResolutionEdge.x) / _ResolutionEdge.y;
				// pIn.color = tex2Dlod(_ColorTexture, float4(edgeUV,0,0));
				// pIn.color.rgb = lerp(_ColorLight, _ColorShadow, dot(tangent, float3(0,1,0)) * 0.5 + 0.5);

				pIn.vertex = mul(UNITY_MATRIX_VP, vertex) + float4(-radius * aspect, -radius * 0.55, 0, 0);
				pIn.texcoord = float2(-0.5,0);
				triStream.Append(pIn);

				pIn.vertex = mul(UNITY_MATRIX_VP, vertex) + float4(0, radius, 0, 0);
				pIn.texcoord = float2(0.5,1.5);
				triStream.Append(pIn);

				pIn.vertex = mul(UNITY_MATRIX_VP, vertex) + float4(radius * aspect, -radius * 0.55, 0, 0);
				pIn.texcoord = float2(1.5,0);
				triStream.Append(pIn);


				// pIn.vertex = mul(UNITY_MATRIX_VP, vertex + float4((tangent * -radius).xy, 0.0, 0));
				// pIn.texcoord = float2(-0.5,0);
				// triStream.Append(pIn);

				// pIn.vertex = mul(UNITY_MATRIX_VP, vertex + float4(float3((tangent * -radius).xy, 0.0) + up * radius * min(4.0, length(velocity)), 0));
				// pIn.texcoord = float2(0.5,1.5);
				// triStream.Append(pIn);

				// pIn.vertex = mul(UNITY_MATRIX_VP, vertex + float4((tangent * radius).xy, 0.0, 0));
				// pIn.texcoord = float2(1.5,0);
				// triStream.Append(pIn);
			}

			float4 frag (FS_INPUT i) : COLOR
			{
				float4 color = i.color;
				float2 uv = i.texcoord;
				// uv.y = 1.0 - uv.y;
				float4 map = tex2D(_MainTex, uv);
				// color.a = step(0.5, map.a) * 0.5;
				color.a = map.a;
				return color;
			}
			ENDCG
		}
	}
}
