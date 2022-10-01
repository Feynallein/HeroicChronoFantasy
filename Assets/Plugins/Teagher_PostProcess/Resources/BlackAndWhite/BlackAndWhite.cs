using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Teagher.Rendering.PostProcessEffects
{
    [Serializable]
    [PostProcess(typeof(BlackAndWhiteRenderer), PostProcessEvent.AfterStack, "Raivk/BlackAndWhite")]
    public sealed class BlackAndWhite : PostProcessEffectSettings
    {
        [Range(0f, 1f)]
        public FloatParameter blend = new FloatParameter { value = 0.0f };

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            return enabled.value
                && blend.value > 0f;
        }
    }

    public sealed class BlackAndWhiteRenderer : PostProcessEffectRenderer<BlackAndWhite>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Raivk/BlackAndWhite"));
            sheet.properties.SetFloat("_Blend", settings.blend);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}