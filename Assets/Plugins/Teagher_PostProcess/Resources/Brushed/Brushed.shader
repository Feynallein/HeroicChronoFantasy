Shader "Raivk/Brushed"
{
	HLSLINCLUDE

#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

	TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	float4 _MainTex_TexelSize;
	float _Blend;
	int _KernelSize;

	struct Region {
		float3 mean;
		float3 variance;
	};

	Region CalcRegion(int2 lower, int2 upper, int samples, float2 uv) {
		Region r;
		float3 sum = 0.0;
		float3 squareSum = 0.0;

		for (int x = lower.x; x <= upper.x; ++x) {
			for (int y = lower.y; y <= upper.y; ++y) {
				float2 offset = float2(_MainTex_TexelSize.x * x, _MainTex_TexelSize.y * y);
				float3 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + offset);
				sum += tex;
				squareSum += tex * tex;
			}
		}

		r.mean = sum / samples;
		float3 variance = abs((squareSum / samples) - (r.mean * r.mean));
		r.variance = length(variance);

		return r;
	}

	float4 Frag(VaryingsDefault i) : SV_Target
	{
		float4 normalColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

		int upper = (_KernelSize - 1) * 0.5f;
		int lower = -upper;

		int samples = (upper + 1) * (upper + 1);

		Region regionA = CalcRegion(int2(lower, lower), int2(0, 0), samples, i.texcoord);
		Region regionB = CalcRegion(int2(0, lower), int2(upper, 0), samples, i.texcoord);
		Region regionC = CalcRegion(int2(lower, 0), int2(0, upper), samples, i.texcoord);
		Region regionD = CalcRegion(int2(0, 0), int2(upper, upper), samples, i.texcoord);

		float3 col = regionA.mean;
		float minVar = regionA.variance;

		float testVal;

		testVal = step(regionB.variance, minVar);
		col = lerp(col, regionB.mean, testVal);
		minVar = lerp(minVar, regionB.variance, testVal);

		testVal = step(regionC.variance, minVar);
		col = lerp(col, regionC.mean, testVal);
		minVar = lerp(minVar, regionC.variance, testVal);

		testVal = step(regionD.variance, minVar);
		col = lerp(col, regionD.mean, testVal);

		return lerp(normalColor, float4(col, 1.0), _Blend);
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