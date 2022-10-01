using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Teagher.Rendering.PostProcessEffects
{
    [Serializable]
    [PostProcess(typeof(CRTRenderer), PostProcessEvent.AfterStack, "Raivk/CRT")]
    public sealed class CRT : PostProcessEffectSettings
    {
        public FloatParameter m_FillColor1 = new FloatParameter() { value = 1.0f };
        public FloatParameter m_FillColor2 = new FloatParameter() { value = 1.0f };
        public FloatParameter m_Contrast = new FloatParameter() { value = 0.0f };
        public FloatParameter m_Brightness = new FloatParameter() { value = 0 };
        public ColorParameter m_ScanLinesColor = new ColorParameter() { value = Color.grey };
        public IntParameter m_ScanLinesInterval = new IntParameter() { value = 0 };
    }

    public sealed class CRTRenderer : PostProcessEffectRenderer<CRT>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Raivk/CRT"));
            sheet.properties.SetFloat("_FillColor1", settings.m_FillColor1);
            sheet.properties.SetFloat("_FillColor2", settings.m_FillColor2);
            sheet.properties.SetFloat("_Contrast", settings.m_Contrast);
            sheet.properties.SetFloat("_Brightness", settings.m_Brightness);
            sheet.properties.SetColor("_ScanLinesColor", settings.m_ScanLinesColor);
            sheet.properties.SetInt("_ScanLinesInterval", settings.m_ScanLinesInterval);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}