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
            textKey.ArgsChanged += OnArgsChanged;
        }

        public virtual void Dispose()
        {
            _localizationProvider.LocalizationChanged -= UpdateLocalization;
            textKey.ArgsChanged -= OnArgsChanged;
        }

        public void SetArgs(params object[] args)
        {
            textKey.SetArgs(args);
        }

        protected virtual void UpdateLocalization()
        {
            label.text = textKey.ToString();
        }

        private void OnArgsChanged(TextKey textKey)
        {
            UpdateLocalization();
        }

        public class Factory : PlaceholderFactory<Label, TextKey, DefaultLocalizedLabelView> { }
    }
}
