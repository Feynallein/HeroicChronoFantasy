using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Teagher.Rendering.PostProcessEffects
{
    [Serializable]
    [PostProcess(typeof(ScreenBordersBlurRenderer), PostProcessEvent.AfterStack, "Raivk/ScreenBordersBlur")]
    public sealed class ScreenBordersBlur : PostProcessEffectSettings
    {
        public IntParameter kernelSize = new IntParameter { value = 1 };
        public FloatParameter spread = new FloatParameter { value = 1 };
    }

    public sealed class ScreenBordersBlurRenderer : PostProcessEffectRenderer<ScreenBordersBlur>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Raivk/ScreenBordersBlur"));
            sheet.properties.SetInt("_KernelSize", settings.kernelSize);
            sheet.properties.SetFloat("_Spread", settings.spread);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}