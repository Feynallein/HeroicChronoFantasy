Shader "Raivk/EdgeLess"
{
	HLSLINCLUDE

#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

	TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	float4 _MainTex_TexelSize;
	float _Blend;
	float _Force;
	int _KernelSize;

	float3 rgb2hsv(float3 c) {
		float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
		float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
		float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

		float d = q.x - min(q.w, q.y);
		float e = 1.0e-10;
		return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
	}

	float3 hsv2rgb(float3 c) {
		c = float3(c.x, clamp(c.yz, 0.0, 10.0));
		float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
		float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
		return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 10.0), c.y);
	}

	float sobel(float2 uv)
	{
		float x = 0;
		float y = 0;

		float2 texelSize = _MainTex_TexelSize;

		x += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(-texelSize.x, -texelSize.y)) * -1.0;
		x += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(-texelSize.x, 0)) * -2.0;
		x += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(-texelSize.x, texelSize.y)) * -1.0;

		x += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(texelSize.x, -texelSize.y)) *  1.0;
		x += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(texelSize.x, 0)) *  2.0;
		x += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(texelSize.x, texelSize.y)) *  1.0;

		y += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(-texelSize.x, -texelSize.y)) * -1.0;
		y += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(0, -texelSize.y)) * -2.0;
		y += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(texelSize.x, -texelSize.y)) * -1.0;

		y += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(-texelSize.x, texelSize.y)) *  1.0;
		y += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(0, texelSize.y)) *  2.0;
		y += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(texelSize.x, texelSize.y)) *  1.0;

		return sqrt(x * x * _Force + y * y * _Force);
	}

	float4 Frag(VaryingsDefault i) : SV_Target
	{
		float4 normalColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

		float edginess = saturate(sobel(i.texcoord));

		float4 modifiedColor = float4(0, 0, 0, 1);

		int upper = ((_KernelSize - 1) / 2);
		int lower = -upper;

		for (int x = lower; x <= upper; ++x)
		{
			for (int y = lower; y <= upper; ++y)
			{
				float2 offset = float2(_MainTex_TexelSize.x * x, _MainTex_TexelSize.y * y);
				modifiedColor += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + offset);
			}
		}

		modifiedColor /= (_KernelSize * _KernelSize);


		return lerp(normalColor, modifiedColor, _Blend * edginess);
	}

		ENDHLSL

		SubShader
	{
		Cull Off ZWrite Off ZTest Always

			Pass
		{
			HLSLPROGRAM

				#pragma vertex VertDefault
				#pragma fragment Frag

			ENDHLSL
		}
	}
}