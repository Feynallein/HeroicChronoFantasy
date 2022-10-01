Shader "Raivk/VideoGlitch"
{
	HLSLINCLUDE

#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

	TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	float _GlitchInterval;
	float _DispProbability;
	float _DispIntensity;
	float _ColorProbability;
	float _ColorIntensity;
	float _DispStripSize;

	//Takes two values and returns a pseudo-random number between 0 (included) and 1 (excluded)
	//It samples the sin function, scales it up (presumably to increase floating point error) and then takes it's fraction part (to get value between 0 and 1)
	float rand(float x, float y) {
		return frac(sin(x*12.9898 + y * 78.233)*43758.5453);
	}

	float4 Frag(VaryingsDefault i) : SV_Target
	{
		//This ensures that the shader only generates new random variables every [_GlitchInterval] seconds, e.g. every 0.5 seconds
		//During each interval the value wether the glitch occurs and how much the glitches stays the same
		float intervalTime = floor(_Time.y / _GlitchInterval) * _GlitchInterval;

		//Second value increased by arbitrary number just to get more possible different random values
		float intervalTime2 = intervalTime + 2.793;

		float timePositionVal = intervalTime;
		float timePositionVal2 = intervalTime2;

		//Random chance that the displacement glich or color glitch occur
		float dispGlitchRandom = rand(timePositionVal, -timePositionVal);
		float colorGlitchRandom = rand(timePositionVal, timePositionVal);

		//Precalculate color channel shift
		float rShiftRandom = (rand(-timePositionVal, timePositionVal) - 0.5) * _ColorIntensity;
		float gShiftRandom = (rand(-timePositionVal, -timePositionVal) - 0.5) * _ColorIntensity;
		float bShiftRandom = (rand(-timePositionVal2, -timePositionVal2) - 0.5) * _ColorIntensity;

		//For the displacement glitch, the image is divided into strips of 0.2 * screen height (5 stripes)
		//This value is the random offset each of the strip boundries get either up or down
		//Without this, each strip would be exactly a 5th of the screen height, with this their height is slightly randomised
		float shiftLineOffset = float((rand(timePositionVal2, timePositionVal2) - 0.5) / 50);

		//If the randomly rolled value is below the probability boundry, apply the displacement effect
		if (dispGlitchRandom < _DispProbability) {
			i.texcoord.x += (rand(floor(i.texcoord.y / (_DispStripSize + shiftLineOffset)) - timePositionVal, floor(i.texcoord.y / (_DispStripSize + shiftLineOffset)) + timePositionVal) - _DispStripSize) * _DispIntensity;
			i.texcoord.x = saturate(i.texcoord.x);
		}

		float4 rShifted = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(i.texcoord.x + rShiftRandom, i.texcoord.y + rShiftRandom));
		float4 gShifted = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(i.texcoord.x + gShiftRandom, i.texcoord.y + gShiftRandom));
		float4 bShifted = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(i.texcoord.x + bShiftRandom, i.texcoord.y + bShiftRandom));

		float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

		//If the randomly rolled value is below the probability boundry and the color effect is turned on, apply the color glitch effect
		//Sets the output color to the shifted r,g,b channels and averages their alpha
		if (colorGlitchRandom < _ColorProbability) {
			color.r = rShifted.r;
			color.g = gShifted.g;
			color.b = bShifted.b;
			color.a = (rShifted.a + gShifted.a + bShifted.a) / 3;
		}
		else {
			color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
		}

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