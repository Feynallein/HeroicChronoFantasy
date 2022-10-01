Shader "Raivk/HSVScreenRemap"
{
	HLSLINCLUDE

#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

	TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	float _Blend;
	float2 _Direction;

	float _HueScale;
	float _SaturationScale;
	float _ValueScale;

	float _HueIntensity;
	float _SaturationIntensity;
	float _ValueIntensity;

	float3 HUEtoRGB(float H)
	{
		float R = abs(H * 6 - 3) - 1;
		float G = 2 - abs(H * 6 - 2);
		float B = 2 - abs(H * 6 - 4);
		return saturate(float3(R, G, B));
	}

	float3 HSVtoRGB(float3 HSV)
	{
		float3 RGB = HUEtoRGB(HSV.x);
		return ((RGB - 1) * HSV.y + 1) * HSV.z;
	}

	float Epsilon = 1e-10;

	float3 RGBtoHCV(float3 RGB)
	{
		// Based on work by Sam Hocevar and Emil Persson
		float4 P = (RGB.g < RGB.b) ? float4(RGB.bg, -1.0, 2.0 / 3.0) : float4(RGB.gb, 0.0, -1.0 / 3.0);
		float4 Q = (RGB.r < P.x) ? float4(P.xyw, RGB.r) : float4(RGB.r, P.yzx);
		float C = Q.x - min(Q.w, Q.y);
		float H = abs((Q.w - Q.y) / (6 * C + Epsilon) + Q.z);
		return float3(H, C, Q.x);
	}

	float3 RGBtoHSV(float3 RGB)
	{
		float3 HCV = RGBtoHCV(RGB);
		float S = HCV.y / (HCV.z + Epsilon);
		return float3(HCV.x, S, HCV.z);
	}

	float4 Frag(VaryingsDefault i) : SV_Target
	{
		float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

		float3 hsvColor = RGBtoHSV(color);

		hsvColor.x = lerp(hsvColor.x, (length(i.texcoord.xy * _Direction) * _HueScale) % 1, _HueIntensity);
		hsvColor.y = lerp(hsvColor.y, (length(i.texcoord.xy * _Direction) * _SaturationScale) % 1, _SaturationIntensity);
		hsvColor.z = lerp(hsvColor.z, (length(i.texcoord.xy * _Direction) * _ValueScale) % 1, _ValueIntensity);

		color.rgb = lerp(color.rgb, HSVtoRGB(hsvColor), _Blend.xxx);
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