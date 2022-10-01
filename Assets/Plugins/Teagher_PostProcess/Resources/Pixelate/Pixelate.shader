Shader "Raivk/Pixelate"
{
	HLSLINCLUDE

		#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

		TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
		float4 _MainTex_TexelSize;
		float _PixelationForce;

		float4 Frag(VaryingsDefault i) : SV_Target
		{
			float2 pixelForce = float2(_PixelationForce, (_MainTex_TexelSize.x / _MainTex_TexelSize.y) * _PixelationForce);

			return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, floor(i.texcoord * pixelForce) / pixelForce);
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