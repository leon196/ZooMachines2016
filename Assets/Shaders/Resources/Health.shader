Shader "Filters/Health" {
	Properties 
	{
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
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "Utils.cginc"
			
			sampler2D _MainTex;
			sampler2D _VertexTexture;
			sampler2D _EdgeTexture;
			sampler2D _ElementTexture;
			float _Speed;
			float _NoiseScale;
			float _RespawnCycle;
			float2 _ResolutionEdge;

			fixed4 frag (v2f_img i) : SV_Target {
				fixed4 element = tex2D(_MainTex, i.uv);
				fixed4 position = tex2D(_VertexTexture, i.uv);
				// element.r = fmod(noiseIQ(position * _NoiseScale) + _Time.x * _Speed, _RespawnCycle);
				float2 currentUV = float2(0,0);
				currentUV.x = fmod(element.r, _ResolutionEdge.x) / _ResolutionEdge.x;
				currentUV.y = floor(element.r / _ResolutionEdge.x) / _ResolutionEdge.y;
				float3 currentTarget = tex2D(_EdgeTexture, currentUV);
				float should = step(distance(position, currentTarget), 0.1);
				element.r += lerp(0, 1, should);
				element.r = fmod(element.r, _ResolutionEdge.x * _ResolutionEdge.y);
				// element.r = 1.0;
				// element.gb = 0.0;
				return element;
			}
			ENDCG
		}
	}
}
