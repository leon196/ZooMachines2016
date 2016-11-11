Shader "Filters/Render"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			sampler2D _ShaderPassTexture;
			sampler2D _EdgeTexture;
			float4 _Color;

			fixed4 frag (v2f_img i) : SV_Target
			{
				fixed4 col = tex2D(_ShaderPassTexture, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
