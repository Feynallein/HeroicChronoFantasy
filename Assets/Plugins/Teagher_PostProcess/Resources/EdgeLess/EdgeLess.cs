using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Teagher.Rendering.PostProcessEffects
{
    [Serializable]
    [PostProcess(typeof(EdgeLessRenderer), PostProcessEvent.AfterStack, "Raivk/EdgeLess")]
    public sealed class EdgeLess : PostProcessEffectSettings
    {
        [Range(0f, 1f)]
        public FloatParameter m_Blend = new FloatParameter() { value = 0.0f };
        public FloatParameter m_Force = new FloatParameter() { value = 1f };
        public IntParameter m_KernelSize = new IntParameter() { value = 3 };

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            return enabled.value
                && m_Blend.value > 0f;
        }
    }

    public sealed class EdgeLessRenderer : PostProcessEffectRenderer<EdgeLess>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Raivk/EdgeLess"));
            sheet.properties.SetFloat("_Blend", settings.m_Blend);
            sheet.properties.SetFloat("_Force", settings.m_Force);
            sheet.properties.SetInt("_KernelSize", settings.m_KernelSize);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}