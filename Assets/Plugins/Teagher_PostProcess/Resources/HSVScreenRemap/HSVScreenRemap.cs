using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Teagher.Rendering.PostProcessEffects
{
    [Serializable]
    [PostProcess(typeof(HSVScreenRemapRenderer), PostProcessEvent.AfterStack, "Raivk/HSVScreenRemap")]
    public sealed class HSVScreenRemap : PostProcessEffectSettings
    {
        [Range(0f, 1f), Tooltip("Color scale effect intensity.")]
        public FloatParameter blend = new FloatParameter { value = 0.0f };

        public Vector2Parameter direction = new Vector2Parameter { value = new Vector2(0, 0) };

        
        public FloatParameter hueScale = new FloatParameter { value = 1.0f };
        public FloatParameter saturationScale = new FloatParameter { value = 1.0f };
        public FloatParameter valueScale = new FloatParameter { value = 1.0f };
        public FloatParameter hueIntensity = new FloatParameter { value = 0.0f };
        public FloatParameter saturationIntensity = new FloatParameter { value = 0.0f };
        public FloatParameter valueIntensity = new FloatParameter { value = 0.0f };

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            return enabled.value
                && blend.value > 0f;
        }
    }

    public sealed class HSVScreenRemapRenderer : PostProcessEffectRenderer<HSVScreenRemap>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Raivk/HSVScreenRemap"));

            sheet.properties.SetFloat("_Blend", settings.blend);
            sheet.properties.SetVector("_Direction", settings.direction);
            sheet.properties.SetFloat("_HueScale", settings.hueScale);
            sheet.properties.SetFloat("_SaturationScale", settings.saturationScale);
            sheet.properties.SetFloat("_ValueScale", settings.valueScale);
            sheet.properties.SetFloat("_HueIntensity", settings.hueIntensity);
            sheet.properties.SetFloat("_SaturationIntensity", settings.saturationIntensity);
            sheet.properties.SetFloat("_ValueIntensity", settings.valueIntensity);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}