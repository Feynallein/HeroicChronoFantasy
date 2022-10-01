using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Teagher.Rendering.PostProcessEffects
{
    [Serializable]
    [PostProcess(typeof(DistanceRecolorrRenderer), PostProcessEvent.AfterStack, "Raivk/DistanceRecolor")]
    public sealed class DistanceRecolor : PostProcessEffectSettings
    {
        [Range(0f, 1f)]
        public FloatParameter m_Blend = new FloatParameter() { value = 0.0f };
        public FloatParameter depthPowerRamp = new FloatParameter { value = 0.75f };
        [ColorUsage(true, true)]
        public TextureParameter m_ColorRamp = new TextureParameter() { value = null };

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            return enabled.value
                && m_Blend.value > 0f;
        }
    }

    public sealed class DistanceRecolorrRenderer : PostProcessEffectRenderer<DistanceRecolor>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Raivk/DistanceRecolor"));
            sheet.properties.SetFloat("_Blend", settings.m_Blend);
            sheet.properties.SetFloat("_DepthPowerRamp", settings.depthPowerRamp);
            var colorRamp = settings.m_ColorRamp.value == null ? RuntimeUtilities.blackTexture : settings.m_ColorRamp.value;
            sheet.properties.SetTexture("_ColorRamp", colorRamp);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}