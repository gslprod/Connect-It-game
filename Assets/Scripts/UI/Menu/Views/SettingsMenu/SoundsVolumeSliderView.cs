using ConnectIt.Audio.Sounds;
using ConnectIt.Localization;
using ConnectIt.UI.CommonViews;
using ConnectIt.UI.CustomControls;
using ConnectIt.Utilities.Extensions;
using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.Views.SettingsMenu
{
    public class SoundsVolumeSliderView : IInitializable, IDisposable
    {
        private readonly ProgressBarSlider _silder;
        private readonly SoundsPlayer _soundsPlayer;
        private readonly DefaultLocalizedTextElementView.Factory _defaultLocalizedTextElementViewFactory;
        private readonly TextKey.Factory _textKeyFactory;

        private DefaultLocalizedTextElementView _titleView;

        public SoundsVolumeSliderView(
            ProgressBarSlider silder,
            SoundsPlayer soundsPlayer,
            DefaultLocalizedTextElementView.Factory defaultLocalizedTextElementViewFactory,
            TextKey.Factory textKeyFactory)
        {
            _silder = silder;
            _soundsPlayer = soundsPlayer;
            _defaultLocalizedTextElementViewFactory = defaultLocalizedTextElementViewFactory;
            _textKeyFactory = textKeyFactory;
        }

        public void Initialize()
        {
            _soundsPlayer.VolumeChanged += OnVolumeChanged;
            _silder.RegisterValueChangedCallback(OnValueChanged);
            _silder.RegisterCallback<PointerCaptureOutEvent>(OnPointerCaptureOut);

            _titleView = _defaultLocalizedTextElementViewFactory.Create(
                _silder.Q<Label>(NameConstants.SettingsMenu.SoundSliderTitleLabel),
                _textKeyFactory.Create(TextKeysConstants.Menu.SettingsMenu.SoundsSlider_Text));

            _silder.highValue = 100f;
            _silder.lowValue = 0f;
            UpdateValue();
        }

        public void Dispose()
        {
            _soundsPlayer.VolumeChanged -= OnVolumeChanged;
            _silder.UnregisterValueChangedCallback(OnValueChanged);
            _silder.UnregisterCallback<PointerCaptureOutEvent>(OnPointerCaptureOut);

            _titleView.Dispose();
        }

        private void UpdateValue()
        {
            _silder.SetValueWithoutNotify(_soundsPlayer.VolumePercents);

            _titleView.SetArgs(new object[] { _soundsPlayer.VolumePercents });
        }

        private void OnVolumeChanged()
        {
            UpdateValue();
        }

        private void OnValueChanged(ChangeEvent<float> changeEvent)
        {
            _soundsPlayer.SetVolumePercent(changeEvent.newValue, false);
        }

        private void OnPointerCaptureOut(PointerCaptureOutEvent pointerUpEvent)
        {
            _soundsPlayer.SetVolumePercent(_silder.value);
        }

        public class Factory : PlaceholderFactory<ProgressBarSlider, SoundsVolumeSliderView> { }
    }
}
