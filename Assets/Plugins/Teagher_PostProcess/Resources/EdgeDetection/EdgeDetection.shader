Shader "Raivk/EdgeDetection"
{
	HLSLINCLUDE

#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

	TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	float4 _MainTex_TexelSize;
	float _Blend;
	float4 _BackgroundColor;
	float4 _EdgeColor;
	float _Force;
	float _PixelColorBlend;
	float _Saturation;
	float _Lightness;

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

		float4 normalColorSaturated = float4(rgb2hsv(normalColor), 1);

		normalColorSaturated.y *= _Saturation;
		normalColorSaturated.z *= _Lightness;

		normalColorSaturated = float4(hsv2rgb(normalColorSaturated), 1);

		float edginess = sobel(i.texcoord);

		float4 color = lerp(_BackgroundColor, lerp(_EdgeColor, _EdgeColor * normalColorSaturated, _PixelColorBlend), edginess);

		return lerp(normalColor, color, _Blend);
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