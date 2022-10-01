using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Teagher.Rendering.PostProcessEffects
{
    [Serializable]
    [PostProcess(typeof(GaussianBlurRenderer), PostProcessEvent.AfterStack, "Raivk/GaussianBlur")]
    public sealed class GaussianBlur : PostProcessEffectSettings
    {
        public IntParameter kernelSize = new IntParameter { value = 1 };
        public FloatParameter spread = new FloatParameter { value = 1 };
    }

    public sealed class GaussianBlurRenderer : PostProcessEffectRenderer<GaussianBlur>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Raivk/GaussianBlur"));
            sheet.properties.SetInt("_KernelSize", settings.kernelSize);
            sheet.properties.SetFloat("_Spread", settings.spread);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}