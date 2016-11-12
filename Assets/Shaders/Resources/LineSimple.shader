Shader "Unlit/LineSimple"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_VertexTexture ("Vertex", 2D) = "white" {}
		_VelocityTexture ("Velocity", 2D) = "white" {}
		_ElementTexture ("Element", 2D) = "white" {}
		_ColorTexture ("Color", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "Queue"="AlphaTest" "RenderType"="Transparent" "IgnoreProjector"="True" }
		Blend SrcAlpha OneMinusSrcAlpha 
		ZTest Off ZWrite Off
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 texcoord2 : TEXCOORD2;
				uint id : SV_VertexID;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 texcoord2 : TEXCOORD1;
				float4 id : TEXCOORD2;
				float4 color : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _VertexTexture;
			sampler2D _ColorTexture;
			sampler2D _ElementTexture;
			float4 _Color;
			float2 _ResolutionEdge;
			float2 _ResolutionColor;
			float _EdgeCount;
			float4x4 _Matrix;
			
			v2f vert (appdata v)
			{
				v2f o;
				float4 vertex = v.vertex; 
				float4 uv = float4(v.texcoord2.xy, 0, 0);
				vertex.xyz = tex2Dlod(_VertexTexture, uv).rgb;
				vertex = mul(_Matrix, vertex);
				o.vertex = UnityObjectToClipPos(vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.texcoord2 = v.texcoord2;

				float4 element = tex2Dlod(_ElementTexture, uv);
				float4 edgeUV = float4(0,0,0,0);
				edgeUV.x = fmod(element.r, _ResolutionEdge.x) / _ResolutionEdge.x;
				edgeUV.y = floor(element.r / _ResolutionEdge.x) / _ResolutionEdge.y;

				o.color = tex2Dlod(_ColorTexture, edgeUV);
				o.id = float4((float)v.id,0,0,0);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float4 color = _Color;
				float2 uv = i.texcoord2;
				// uv.y = 1.0 - uv.y;

				// float2 edgeUV = float2(0,0);
				// float total = _ResolutionEdge.x * _ResolutionEdge.y;
				// // element.r = fmod(abs(element.r + 1), _EdgeCount);// + _EdgeCount), _EdgeCount);
				// edgeUV.x = fmod(element.r, _ResolutionEdge.x) / _ResolutionEdge.x;
				// edgeUV.y = floor(element.r / _ResolutionEdge.x) / _ResolutionEdge.y;
				color.rgb *= i.color;
				// color.rgb = float3(1,1,1) * frac(i.id / 10.);
				return color;
			}
			ENDCG
		}
	}
}
