using ConnectIt.Localization;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.CommonViews
{
    public class DefaultUniversalTextElementView : DefaultLocalizedTextElementView
    {
        protected string text;

        public DefaultUniversalTextElementView(
            TextElement textElement,
            TextKey textKey,
            string text,
            ILocalizationProvider localizationProvider) : base(textElement, textKey, localizationProvider)
        {
            this.text = text;
        }

        public override void Initialize()
        {
            SetLocalizationEnable(textKey != null);
            UpdateLabel();
        }

        protected override void UpdateLabel()
        {
            if (LocalizationEnabled)
                base.UpdateLabel();
            else
                textElement.text = text;
        }

        public new class Factory : PlaceholderFactory<TextElement, TextKey, string, DefaultUniversalTextElementView> { }
    }
}
