using ConnectIt.Audio.OST;
using ConnectIt.Localization;
using ConnectIt.UI.CommonViews;
using ConnectIt.UI.CustomControls;
using ConnectIt.Utilities.Extensions;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.UI.Menu.Views.SettingsMenu
{
    public class OSTVolumeSliderView : IInitializable, IDisposable
    {
        private readonly ProgressBarSlider _silder;
        private readonly OSTPlayer _ostPlayer;
        private readonly DefaultLocalizedTextElementView.Factory _defaultLocalizedTextElementViewFactory;
        private readonly TextKey.Factory _textKeyFactory;

        private DefaultLocalizedTextElementView _titleView;

        public OSTVolumeSliderView(
            ProgressBarSlider silder,
            OSTPlayer ostPlayer,
            DefaultLocalizedTextElementView.Factory defaultLocalizedTextElementViewFactory,
            TextKey.Factory textKeyFactory)
        {
            _silder = silder;
            _ostPlayer = ostPlayer;
            _defaultLocalizedTextElementViewFactory = defaultLocalizedTextElementViewFactory;
            _textKeyFactory = textKeyFactory;
        }

        public void Initialize()
        {
            _ostPlayer.VolumeChanged += OnVolumeChanged;
            _silder.RegisterValueChangedCallback(OnValueChanged);
            _silder.RegisterCallback<PointerCaptureOutEvent>(OnPointerCaptureOut);

            _titleView = _defaultLocalizedTextElementViewFactory.Create(
                _silder.Q<Label>(NameConstants.SettingsMenu.MusicSliderTitleLabel),
                _textKeyFactory.Create(TextKeysConstants.Menu.SettingsMenu.MusicSlider_Text)); 

            _silder.highValue = 100f;
            _silder.lowValue = 0f;
            UpdateValue();
        }

        public void Dispose()
        {
            _ostPlayer.VolumeChanged -= OnVolumeChanged;
            _silder.UnregisterValueChangedCallback(OnValueChanged);
            _silder.UnregisterCallback<PointerCaptureOutEvent>(OnPointerCaptureOut);

            _titleView.Dispose();
        }

        private void UpdateValue()
        {
            _silder.SetValueWithoutNotify(_ostPlayer.VolumePercents);

            _titleView.SetArgs(new object[] { _ostPlayer.VolumePercents });
        }

        private void OnVolumeChanged()
        {
            UpdateValue();
        }

        private void OnValueChanged(ChangeEvent<float> changeEvent)
        {
            _ostPlayer.SetVolumePercent(changeEvent.newValue, false);
        }

        private void OnPointerCaptureOut(PointerCaptureOutEvent pointerUpEvent)
        {
            _ostPlayer.SetVolumePercent(_silder.value);
        }

        public class Factory : PlaceholderFactory<ProgressBarSlider, OSTVolumeSliderView> { }
    }
}
