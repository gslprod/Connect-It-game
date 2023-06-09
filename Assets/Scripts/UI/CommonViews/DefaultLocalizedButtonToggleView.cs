﻿using ConnectIt.Audio.Sounds;
using ConnectIt.Config;
using ConnectIt.Localization;
using ConnectIt.UI.CustomControls;
using ConnectIt.Utilities.Extensions;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.CommonViews
{
    public class DefaultLocalizedButtonToggleView : DefaultLocalizedTextElementView
    {
        public bool Value => toggle.value;

        private readonly TextKey.Factory _textKeyFactory;

        protected readonly SoundsPlayer soundsPlayer;
        protected readonly AudioConfig audioConfig;
        protected TextKey enabledTextKey;
        protected TextKey disabledTextKey;
        protected ButtonToggle toggle;

        public DefaultLocalizedButtonToggleView(
            ButtonToggle toggle,
            TextKey textKey,
            TextKey enabledTextKey,
            TextKey disabledTextKey,
            ILocalizationProvider localizationProvider,
            TextKey.Factory textKeyFactory,
            SoundsPlayer soundsPlayer,
            AudioConfig audioConfig) : base(toggle, textKey, localizationProvider)
        {
            this.toggle = toggle;
            this.enabledTextKey = enabledTextKey;
            this.disabledTextKey = disabledTextKey;
            _textKeyFactory = textKeyFactory;
            this.soundsPlayer = soundsPlayer;
            this.audioConfig = audioConfig;
        }

        public override void Initialize()
        {
            enabledTextKey ??= _textKeyFactory.Create(TextKeysConstants.Common.On);
            disabledTextKey ??= _textKeyFactory.Create(TextKeysConstants.Common.Off);

            UpdateArgs(toggle.value);
            toggle.RegisterCallback<ChangeEvent<bool>>(ToggleValueChanged);

            base.Initialize();
        }

        public override void Dispose()
        {
            toggle.UnregisterCallback<ChangeEvent<bool>>(ToggleValueChanged);

            base.Dispose();
        }

        private void ToggleValueChanged(ChangeEvent<bool> changeEvent)
        {
            UpdateArgs(changeEvent.newValue);

            soundsPlayer.Play(audioConfig.Click, SoundMixerGroup.UI);
        }

        private void UpdateArgs(bool toggleValue)
        {
            textKey.SetArgs(new object[]
            {
                toggleValue ? enabledTextKey : disabledTextKey
            });
        }

        public new class Factory : PlaceholderFactory<ButtonToggle, TextKey, TextKey, TextKey, DefaultLocalizedButtonToggleView> { }
    }
}
