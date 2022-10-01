using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Teagher.Rendering.PostProcessEffects
{
    [Serializable]
    [PostProcess(typeof(SepiaRenderer), PostProcessEvent.AfterStack, "Raivk/Sepia")]
    public sealed class Sepia : PostProcessEffectSettings
    {
        [Range(0f, 1f), Tooltip("Sepia effect intensity.")]
        public FloatParameter blend = new FloatParameter { value = 0.0f };

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            return enabled.value
                && blend.value > 0f;
        }
    }

    public sealed class SepiaRenderer : PostProcessEffectRenderer<Sepia>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Raivk/Sepia"));
            sheet.properties.SetFloat("_Blend", settings.blend);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}