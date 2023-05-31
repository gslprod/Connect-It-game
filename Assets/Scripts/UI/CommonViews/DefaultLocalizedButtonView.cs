using ConnectIt.Audio.Sounds;
using ConnectIt.Config;
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
            ILocalizationProvider localizationProvider,
            SoundsPlayer soundsPlayer,
            AudioConfig audioConfig) : base(button, onClick, soundsPlayer, audioConfig)
        {
            this.textKey = textKey;
            _localizationProvider = localizationProvider;
        }

        public override void Initialize()
        {
            UpdateLocalization();

            _localizationProvider.LocalizationChanged += UpdateLocalization;
            textKey.ArgsChanged += OnArgsChanged;

            base.Initialize();
        }

        public override void Dispose()
        {
            _localizationProvider.LocalizationChanged -= UpdateLocalization;
            textKey.ArgsChanged -= OnArgsChanged;

            base.Dispose();
        }

        public void SetArgs(params object[] args)
        {
            textKey.SetArgs(args);
        }

        protected virtual void UpdateLocalization()
        {
            button.text = textKey.ToString();
        }

        private void OnArgsChanged(TextKey textKey)
        {
            UpdateLocalization();
        }

        public new class Factory : PlaceholderFactory<Button, Action, TextKey, DefaultLocalizedButtonView> { }
    }
}
