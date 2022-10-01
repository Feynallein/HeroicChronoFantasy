using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Teagher.Rendering.PostProcessEffects
{
    [Serializable]
    [PostProcess(typeof(BoxBlurRenderer), PostProcessEvent.AfterStack, "Raivk/BoxBlur")]
    public sealed class BoxBlur : PostProcessEffectSettings
    {
        public IntParameter kernelSize = new IntParameter { value = 1 };
    }

    public sealed class BoxBlurRenderer : PostProcessEffectRenderer<BoxBlur>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Raivk/BoxBlur"));
            sheet.properties.SetInt("_KernelSize", settings.kernelSize);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}