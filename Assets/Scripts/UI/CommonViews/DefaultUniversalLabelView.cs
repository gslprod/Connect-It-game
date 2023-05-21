using ConnectIt.Localization;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.CommonViews
{
    public class DefaultUniversalLabelView : DefaultLocalizedLabelView
    {
        protected string text;

        public DefaultUniversalLabelView(
            Label label,
            TextKey textKey,
            string text,
            ILocalizationProvider localizationProvider) : base(label, textKey, localizationProvider)
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
                label.text = text;
        }

        public new class Factory : PlaceholderFactory<Label, TextKey, string, DefaultUniversalLabelView> { }
    }
}
