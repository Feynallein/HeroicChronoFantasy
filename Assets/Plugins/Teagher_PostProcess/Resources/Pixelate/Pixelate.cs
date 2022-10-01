using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Teagher.Rendering.PostProcessEffects
{
    [Serializable]
    [PostProcess(typeof(PixelateRenderer), PostProcessEvent.AfterStack, "Raivk/Pixelate")]
    public sealed class Pixelate : PostProcessEffectSettings
    {
        public FloatParameter m_Pixels = new FloatParameter { value = 2048f };

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            return enabled.value
                && m_Pixels.value > 0f;
        }
    }

    public sealed class PixelateRenderer : PostProcessEffectRenderer<Pixelate>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Raivk/Pixelate"));
            sheet.properties.SetFloat("_PixelationForce", settings.m_Pixels);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}