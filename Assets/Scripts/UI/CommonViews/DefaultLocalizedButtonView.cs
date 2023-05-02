using ConnectIt.Localization;
using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.CommonViews
{
    public class DefaultLocalizedButtonView : DefaultButtonView
    {
        protected TextKey textKey;

        private readonly ILocalizationProvider _localizationProvider;

        public DefaultLocalizedButtonView(Button button,
            Action onClick,
            TextKey textKey,
            ILocalizationProvider localizationProvider) : base(button, onClick)
        {
            this.textKey = textKey;
            _localizationProvider = localizationProvider;
        }

        public override void Initialize()
        {
            UpdateLocalization();

            _localizationProvider.LocalizationChanged += UpdateLocalization;

            base.Initialize();
        }

        public override void Dispose()
        {
            _localizationProvider.LocalizationChanged -= UpdateLocalization;

            base.Dispose();
        }

        protected virtual void UpdateLocalization()
        {
            button.text = textKey.ToString();
        }

        public new class Factory : PlaceholderFactory<Button, Action, TextKey, DefaultLocalizedButtonView> { }
    }
}
