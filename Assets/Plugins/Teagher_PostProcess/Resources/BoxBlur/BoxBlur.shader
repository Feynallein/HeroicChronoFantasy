Shader "Raivk/BoxBlur"
{
	HLSLINCLUDE

#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

	TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	float4 _MainTex_TexelSize;
	int _KernelSize;

	float4 Frag(VaryingsDefault i) : SV_Target
	{
		float3 color = float3(0.0, 0.0, 0.0);

		int upper = ((_KernelSize - 1) / 2);
		int lower = -upper;

		for (int x = lower; x <= upper; ++x)
		{
			for (int y = lower; y <= upper; ++y)
			{
				float2 offset = float2(_MainTex_TexelSize.x * x, _MainTex_TexelSize.y * y);
				color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + offset);
			}
		}

		color /= (_KernelSize * _KernelSize);
		return float4(color, 1.0f);
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