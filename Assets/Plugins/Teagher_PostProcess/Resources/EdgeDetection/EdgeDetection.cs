using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Teagher.Rendering.PostProcessEffects
{
    [Serializable]
    [PostProcess(typeof(EdgeDetectionRenderer), PostProcessEvent.AfterStack, "Raivk/EdgeDetection")]
    public sealed class EdgeDetection : PostProcessEffectSettings
    {
        [Range(0f, 1f)]
        public FloatParameter m_Blend = new FloatParameter() { value = 0.0f };
        public FloatParameter m_Force = new FloatParameter() { value = 1f };
        [ColorUsage(true, true)]
        public ColorParameter m_BackgroundColor = new ColorParameter() { value = Color.black };
        [ColorUsage(true, true)]
        public ColorParameter m_EdgeColor = new ColorParameter() { value = Color.white };

        [Range(0f, 1f)]
        public FloatParameter m_PixelColorBlend = new FloatParameter() { value = 0f };
        public FloatParameter m_SaturationForce = new FloatParameter() { value = 1f };
        public FloatParameter m_LightnessForce = new FloatParameter() { value = 1f };

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            return enabled.value
                && m_Blend.value > 0f;
        }
    }

    public sealed class EdgeDetectionRenderer : PostProcessEffectRenderer<EdgeDetection>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Raivk/EdgeDetection"));
            sheet.properties.SetFloat("_Blend", settings.m_Blend);
            sheet.properties.SetFloat("_Force", settings.m_Force);
            sheet.properties.SetColor("_BackgroundColor", settings.m_BackgroundColor);
            sheet.properties.SetColor("_EdgeColor", settings.m_EdgeColor);
            sheet.properties.SetFloat("_PixelColorBlend", settings.m_PixelColorBlend);
            sheet.properties.SetFloat("_Saturation", settings.m_SaturationForce);
            sheet.properties.SetFloat("_Lightness", settings.m_LightnessForce);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}