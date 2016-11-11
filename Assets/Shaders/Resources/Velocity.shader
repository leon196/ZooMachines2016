Shader "Filters/Velocity" {
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_VertexTexture ("Vertex", 2D) = "white" {}
		_VertexInitialTexture ("Vertex Initial", 2D) = "white" {}
		_VelocityTexture ("Velocity", 2D) = "white" {}
		_ElementTexture ("Element", 2D) = "white" {}

		_GlobalSpeed ("Global Speed", Float) = 0.02
		_Fade ("Fade", Float) = 0.9
		_SpeedAttractor ("Speed Attractor", Float) = 1
		_ScaleAttractor ("Scale Attractor", Float) = 0.05
		_SpeedNoise ("Speed Noise", Float) = 0
		_ScaleNoise ("Scale Noise", Vector) = (5, 2, 10, 0)
		_TimeSpeedNoise ("Time Speed Noise", Vector) = (1, 0.1, 0.4, 0)
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
			sampler2D _VelocityTexture;
			sampler2D _VertexTexture;
			sampler2D _EdgeTexture;
			sampler2D _ElementTexture;
			float _GlobalSpeed;
			float _Fade;
			float _SpeedAttractor;
			float _SpeedNoise;
			float _ScaleAttractor;
			float2 _ResolutionEdge;
			float3 _ScaleNoise;
			float3 _TimeSpeedNoise;

			fixed4 frag (v2f_img i) : SV_Target {
				fixed4 velocity = tex2D(_MainTex, i.uv);
				fixed4 position = tex2D(_VertexTexture, i.uv);
				float4 element = tex2D(_ElementTexture, i.uv);
				float4 e = float4(1,1,1,1) * 0.00001;

				// float x = position.x * _ScaleAttractor;
				// float y = position.y * _ScaleAttractor;
				// float z = position.z * _ScaleAttractor;

				float3 offset = float3(0,0,0);
				offset.x += noiseIQ(position.xyz * _ScaleNoise.x + float3(_Time.y * _TimeSpeedNoise.x,0,0));
				offset.y += noiseIQ(position.xyz * _ScaleNoise.y + float3(_Time.y * _TimeSpeedNoise.y,0,0));
				offset.z += noiseIQ(position.xyz * _ScaleNoise.z + float3(_Time.y * _TimeSpeedNoise.z,0,0));

				float2 edgeUV = float2(0,0);
				edgeUV.x = fmod(element.r, _ResolutionEdge.x) / _ResolutionEdge.x;
				edgeUV.y = floor(element.r / _ResolutionEdge.x) / _ResolutionEdge.y;

				float3 dir = tex2D(_EdgeTexture, edgeUV) - position;
				velocity.xyz += dir * _GlobalSpeed;
				// velocity.xyz = burke;
				// velocity.xyz = aizawa * _SpeedAttractor;
				// velocity.xyz = arneodo;
				// velocity.xyz += offset * _SpeedNoise * _GlobalSpeed;
				// velocity.xyz = normalize(velocity.xyz)* _GlobalSpeed;
				velocity.xyz *= _Fade;
				// velocity.xyz = lerp(velocity, dir, step(length(dir), 0.01));
				return velocity;
			}
			ENDCG
		}
	}
}
