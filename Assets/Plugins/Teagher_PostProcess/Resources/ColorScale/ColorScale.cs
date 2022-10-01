using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Teagher.Rendering.PostProcessEffects
{
    [Serializable]
    [PostProcess(typeof(ColorScaleRenderer), PostProcessEvent.AfterStack, "Raivk/ColorScale")]
    public sealed class ColorScale : PostProcessEffectSettings
    {
        [Range(0f, 1f), Tooltip("Color scale effect intensity.")]
        public FloatParameter blend = new FloatParameter { value = 0.0f };
        [ColorUsage(true, true)] public ColorParameter lowColor = new ColorParameter { value = Color.black };
        [ColorUsage(true, true)] public ColorParameter highColor = new ColorParameter { value = Color.white };

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            return enabled.value
                && blend.value > 0f;
        }
    }

    public sealed class ColorScaleRenderer : PostProcessEffectRenderer<ColorScale>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Raivk/ColorScale"));
            sheet.properties.SetFloat("_Blend", settings.blend);
            sheet.properties.SetColor("_LowColor", settings.lowColor);
            sheet.properties.SetColor("_HighColor", settings.highColor);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}