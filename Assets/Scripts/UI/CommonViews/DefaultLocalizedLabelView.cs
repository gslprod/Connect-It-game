using ConnectIt.Localization;
using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.CommonViews
{
    public class DefaultLocalizedLabelView : IInitializable, IDisposable
    {
        private readonly ILocalizationProvider _localizationProvider;

        protected Label label;
        protected TextKey textKey;

        public DefaultLocalizedLabelView(Label label,
            TextKey textKey,
            ILocalizationProvider localizationProvider)
        {
            this.label = label;
            this.textKey = textKey;
            _localizationProvider = localizationProvider;
        }

        public virtual void Initialize()
        {
            UpdateLocalization();

            _localizationProvider.LocalizationChanged += UpdateLocalization;
        }

        public virtual void Dispose()
        {
            _localizationProvider.LocalizationChanged -= UpdateLocalization;
        }

        protected virtual void UpdateLocalization()
        {
            label.text = textKey.ToString();
        }

        public class Factory : PlaceholderFactory<Label, TextKey, DefaultLocalizedLabelView> { }
    }
}
