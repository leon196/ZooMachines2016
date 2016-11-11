Shader "Filters/Filter"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_Fade ("Fade", Range(0,1)) = 0.1
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
			sampler2D _CameraTexture;
			float4 _Color;
			float _Fade;

			fixed4 frag (v2f_img i) : SV_Target
			{
				fixed4 col = tex2D(_CameraTexture, i.uv);
				col = lerp(col, tex2D(_MainTex, i.uv), _Fade);
				// col.rgb += camera.rgb * 0.1;
				// col.rgb += tex2D(_MainTex, i.uv) * _Fade;
				return col;
			}
			ENDCG
		}
	}
}
