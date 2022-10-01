using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Teagher.Rendering.PostProcessEffects
{
    [Serializable]
    [PostProcess(typeof(BrushedRenderer), PostProcessEvent.AfterStack, "Raivk/Brushed")]
    public sealed class Brushed : PostProcessEffectSettings
    {
        [Range(0f, 1f)]
        public FloatParameter blend = new FloatParameter { value = 0.0f };

        public IntParameter kernelSize = new IntParameter() { value = 17 };

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            return enabled.value
                && blend.value > 0f;
        }
    }

    public sealed class BrushedRenderer : PostProcessEffectRenderer<Brushed>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Raivk/Brushed"));
            sheet.properties.SetFloat("_Blend", settings.blend);
            sheet.properties.SetInt("_KernelSize", settings.kernelSize);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}