Shader "Raivk/ScreenFading"
{
	HLSLINCLUDE

		#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

		TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
		TEXTURE2D_SAMPLER2D(_Texture, sampler_Texture);
		float _Blend;
		float4 _ScreenColor;
		float _Distort;
		float _Fade;
		

		float4 Frag(VaryingsDefault i) : SV_Target
		{
			float4 transit = SAMPLE_TEXTURE2D(_Texture, sampler_Texture, i.texcoord);

			float2 direction = float2(0,0);
			if(_Distort)
				direction = normalize(float2((transit.r - 0.5) * 2, (transit.g - 0.5) * 2));

			float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + _Blend * direction);

			if (transit.b < _Blend)
				return col = lerp(col, _ScreenColor, _Fade);

			return col;
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