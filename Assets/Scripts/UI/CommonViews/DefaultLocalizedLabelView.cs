using ConnectIt.Localization;
using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.CommonViews
{
    public class DefaultLocalizedLabelView : IInitializable, IDisposable
    {
        public bool LocalizationEnabled { get; private set; } = false;

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
            SetLocalizationEnable(true);
            UpdateLabel();
        }

        public virtual void Dispose()
        {
            if (LocalizationEnabled)
                Unsubscribe();
        }

        public void SetArgs(params object[] args)
        {
            textKey.SetArgs(args);
        }

        protected void SetLocalizationEnable(bool isEnable)
        {
            if (LocalizationEnabled == isEnable)
                return;

            LocalizationEnabled = isEnable;

            if (isEnable)
            {
                Subscribe();
                UpdateLabel();
            }
            else
            {
                Unsubscribe();
            }
        }

        protected virtual void UpdateLabel()
        {
            label.text = textKey.ToString();
        }

        private void OnArgsChanged(TextKey textKey)
        {
            UpdateLabel();
        }

        private void Subscribe()
        {
            _localizationProvider.LocalizationChanged += UpdateLabel;
            textKey.ArgsChanged += OnArgsChanged;
        }

        private void Unsubscribe()
        {
            _localizationProvider.LocalizationChanged -= UpdateLabel;
            textKey.ArgsChanged -= OnArgsChanged;
        }

        public class Factory : PlaceholderFactory<Label, TextKey, DefaultLocalizedLabelView> { }
    }
}
