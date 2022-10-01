using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Teagher.Rendering.PostProcessEffects
{
    [Serializable]
    [PostProcess(typeof(ReducedColorPaletteRenderer), PostProcessEvent.AfterStack, "Raivk/ReducedColorPalette")]
    public sealed class ReducedColorPalette : PostProcessEffectSettings
    {
        public IntParameter ColorPalettePrecision = new IntParameter() { value = 4096 };
    }

    public sealed class ReducedColorPaletteRenderer : PostProcessEffectRenderer<ReducedColorPalette>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Raivk/ReducedColorPalette"));
            sheet.properties.SetFloat("_Precision", settings.ColorPalettePrecision);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}