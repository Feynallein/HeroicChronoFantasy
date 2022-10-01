Shader "Raivk/Sepia"
{
	HLSLINCLUDE

#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

	TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	float _Blend;

	float4 Frag(VaryingsDefault i) : SV_Target
	{
		const half3x3 sepiaVals = half3x3
		(
			0.393, 0.349, 0.272,    // Red
			0.769, 0.686, 0.534,    // Green
			0.189, 0.168, 0.131     // Blue
		);
		
		float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
		half3 result = mul(color.rgb, sepiaVals);
		return lerp(color, float4(result.rgb, 1), _Blend);
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