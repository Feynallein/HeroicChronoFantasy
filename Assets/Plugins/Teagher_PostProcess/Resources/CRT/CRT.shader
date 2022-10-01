Shader "Raivk/CRT"
{
	HLSLINCLUDE

#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

	TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	float _FillColor1;
	float _FillColor2;
	float _Contrast;
	float _Brightness;
	float4 _ScanLinesColor;
	int _ScanLinesInterval;

	float4 Frag(VaryingsDefault i) : SV_Target
	{
		float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
		float2 pixelCoordinates = i.texcoord * _ScreenParams.xy;

		color += (_Brightness / 255);

		uint pp = (uint)pixelCoordinates.x % 3;

		float4 muls = float4(0, 0, 0, 1);

		if (pp == 1) {
			muls.r = 1; muls.g = _FillColor1; muls.b = _FillColor2;
		}
		else if (pp == 2) {
			muls.g = 1; muls.b = _FillColor1; muls.r = _FillColor2;
		}
		else {
			muls.b = 1; muls.r = _FillColor1; muls.g = _FillColor2;
		}

		if ((uint)pixelCoordinates.y % _ScanLinesInterval == 0) muls *= _ScanLinesColor;

		color = color * muls;

		color = color - _Contrast * (color - 1.0) * color * (color - 0.5);

		return color;
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