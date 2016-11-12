Shader "Unlit/LineSimple"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
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
				float4 texcoord2 : TEXCOORD1;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _VertexTexture;
			sampler2D _ColorTexture;
			sampler2D _ElementTexture;
			float4 _Color;
			float2 _ResolutionEdge;
			float4x4 _Matrix;
			
			v2f vert (appdata v)
			{
				v2f o;
				float4 vertex = v.vertex; 
				vertex.xyz = tex2Dlod(_VertexTexture, float4(v.texcoord2.xy, 0, 0)).rgb;
				vertex = mul(_Matrix, vertex);
				o.vertex = UnityObjectToClipPos(vertex);
				// o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv = v.texcoord2;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float4 color = _Color;
				float4 element = tex2D(_ElementTexture, i.uv);

				float2 edgeUV = float2(0,0);
				edgeUV.x = fmod(element.r, _ResolutionEdge.x) / _ResolutionEdge.x;
				edgeUV.y = floor(element.r / _ResolutionEdge.x) / _ResolutionEdge.y;

				color *= tex2D(_ColorTexture, edgeUV);
				return color;
			}
			ENDCG
		}
	}
}
