Shader "Filters/Position" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_VertexTexture ("Vertex", 2D) = "white" {}
		_VertexInitialTexture ("Vertex Initial", 2D) = "white" {}
		_VelocityTexture ("Velocity", 2D) = "white" {}
		_ElementTexture ("Element", 2D) = "white" {}
	}
	SubShader {
		Cull Off ZWrite Off ZTest Always
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "Utils.cginc"
			
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _VelocityTexture;
			sampler2D _VertexInitialTexture;
			sampler2D _ElementTexture;
			sampler2D _EdgeTexture;
			float2 _Resolution;
			float2 _ResolutionEdge;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				uint id : SV_VertexID;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 id : TEXCOORD1;
			};
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.id = float4((float)v.id,0,0,0);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target {
				fixed4 position = tex2D(_MainTex, i.uv);
				fixed4 element = tex2D(_ElementTexture, i.uv);
				fixed4 original = tex2D(_VertexInitialTexture, i.uv);
				float3 offset = tex2D(_VelocityTexture, i.uv).xyz;


				// float2 edgeUV = float2(0,0);
				// float v = sin(_Time.y + noiseIQ(original) * PI) * 0.5 + 0.5;
				// float v = index / (_Resolution.x * _Resolution.y);
				// v = floor(v * _ResolutionEdge.x * _ResolutionEdge.y);
				// edgeUV.x = fmod(v, _ResolutionEdge.x) / _ResolutionEdge.x;
				// edgeUV.y = floor(v / _ResolutionEdge.x) / _ResolutionEdge.y;
				// position.xyz = tex2D(_EdgeTexture, edgeUV);
				position.xyz += offset;
				// position.xyz = original.xyz;
				// float should = step(i.id.x, 0.01);
				// float3 follow = lerp(position, tex2D(_MainTex, previous), 0.9);//lerp(0.9, 1.0, indexRatio));
				// position.xyz = lerp(follow, position + offset, should);

				position.xyz = lerp(position, original, step(200.0, length(position.xyz)));

				return position;
			}
			ENDCG
		}
	}
}
