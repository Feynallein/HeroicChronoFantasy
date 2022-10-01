using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Teagher.Rendering.PostProcessEffects
{
    [Serializable]
    [PostProcess(typeof(ScreenFadingRenderer), PostProcessEvent.AfterStack, "Raivk/ScreenFading")]
    public sealed class ScreenFading : PostProcessEffectSettings
    {
        [Range(0f, 1f)]
        public FloatParameter blend = new FloatParameter { value = 0.0f };

        public TextureParameter texture = new TextureParameter { value = null };

        public ColorParameter screenColor = new ColorParameter() { value = Color.black };

        public BoolParameter distort = new BoolParameter { value = false };

        [Range(0f, 1f)]
        public FloatParameter fade = new FloatParameter { value = 1 };

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            return enabled.value
                && blend.value > 0f && texture != null;
        }
    }

    public sealed class ScreenFadingRenderer : PostProcessEffectRenderer<ScreenFading>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Raivk/ScreenFading"));
            sheet.properties.SetFloat("_Blend", settings.blend);
            if (settings.texture.value) sheet.properties.SetTexture("_Texture", settings.texture);
            sheet.properties.SetColor("_ScreenColor", settings.screenColor);
            sheet.properties.SetFloat("_Distort", settings.distort ? 1 : 0);
            sheet.properties.SetFloat("_Fade", settings.fade);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}