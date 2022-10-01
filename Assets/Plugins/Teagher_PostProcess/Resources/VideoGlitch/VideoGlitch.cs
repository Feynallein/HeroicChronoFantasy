using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Teagher.Rendering.PostProcessEffects
{
    [Serializable]
    [PostProcess(typeof(VideoGlitchRenderer), PostProcessEvent.AfterStack, "Raivk/VideoGlitch")]
    public sealed class VideoGlitch : PostProcessEffectSettings
    {
        public FloatParameter m_GlitchInterval = new FloatParameter { value = 0.5f };
        public FloatParameter m_DispProbability = new FloatParameter { value = 0.0f };
        public FloatParameter m_DispIntensity = new FloatParameter { value = 0.0f };
        public FloatParameter m_ColorProbability = new FloatParameter { value = 0.0f };
        public FloatParameter m_ColorIntensity = new FloatParameter { value = 0.0f };
        public FloatParameter m_DispStripSize = new FloatParameter { value = 0.2f };

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            return enabled.value
                && (m_DispProbability.value > 0f
                || m_DispIntensity.value > 0f
                || m_ColorProbability > 0f
                || m_ColorIntensity > 0f);
        }
    }

    public sealed class VideoGlitchRenderer : PostProcessEffectRenderer<VideoGlitch>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Raivk/VideoGlitch"));
            sheet.properties.SetFloat("_GlitchInterval", settings.m_GlitchInterval);
            sheet.properties.SetFloat("_DispProbability", settings.m_DispProbability);
            sheet.properties.SetFloat("_DispIntensity", settings.m_DispIntensity);
            sheet.properties.SetFloat("_ColorProbability", settings.m_ColorProbability);
            sheet.properties.SetFloat("_ColorIntensity", settings.m_ColorIntensity);
            sheet.properties.SetFloat("_DispStripSize", settings.m_DispStripSize);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}