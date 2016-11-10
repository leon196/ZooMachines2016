Shader "Filters/FilterWave"
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
			float4 _Color;

			fixed4 frag (v2f_img i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				// col.rgb = lerp(col, _Color, 0.01 / Luminance(col.rgb));
				// col.rgb = 0.01 / Luminance(col.rgb);

				float x = i.uv.x;
				float y = i.uv.y;
				float wave = 0.01 / abs((y - 0.5) * 10. - sin(x * 10.));
				col.rgb = lerp(col, _Color, wave);
				return col;
			}
			ENDCG
		}
	}
}
