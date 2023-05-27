using ConnectIt.Localization;
using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.CommonViews
{
    public class DefaultLocalizedTextElementView : IInitializable, IDisposable
    {
        public bool LocalizationEnabled { get; private set; } = false;
        public TextElement TextElement => textElement;

        private readonly ILocalizationProvider _localizationProvider;

        protected TextElement textElement;
        protected TextKey textKey;

        public DefaultLocalizedTextElementView(TextElement textElement,
            TextKey textKey,
            ILocalizationProvider localizationProvider)
        {
            this.textElement = textElement;
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

        public void SetTextKey(TextKey newTextKey)
        {
            if (LocalizationEnabled)
                Unsubscribe();

            textKey = newTextKey;

            if (LocalizationEnabled)
                Subscribe();

            UpdateLabel();
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
            textElement.text = textKey.ToString();
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

        public class Factory : PlaceholderFactory<TextElement, TextKey, DefaultLocalizedTextElementView> { }
    }
}
