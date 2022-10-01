using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Teagher.Rendering.PostProcessEffects
{
    [Serializable]
    [PostProcess(typeof(SilhouetteRenderer), PostProcessEvent.AfterStack, "Raivk/Silhouette")]
    public sealed class Silhouette : PostProcessEffectSettings
    {
        [Range(0f, 1f)]
        public FloatParameter blend = new FloatParameter { value = 0.0f };
        public FloatParameter depthPowerRamp = new FloatParameter { value = 0.75f };
        public ColorParameter nearColor = new ColorParameter { value = Color.black };
        public ColorParameter farColor = new ColorParameter { value = Color.white };

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            return enabled.value
                && blend.value > 0f;
        }
    }

    public sealed class SilhouetteRenderer : PostProcessEffectRenderer<Silhouette>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Raivk/Silhouette"));
            sheet.properties.SetFloat("_Intensity", settings.blend);
            sheet.properties.SetFloat("_DepthPowerRamp", settings.depthPowerRamp);
            sheet.properties.SetColor("_NearColor", settings.nearColor);
            sheet.properties.SetColor("_FarColor", settings.farColor);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}