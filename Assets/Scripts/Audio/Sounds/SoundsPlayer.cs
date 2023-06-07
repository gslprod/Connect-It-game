using ConnectIt.Save.SaveProviders;
using ConnectIt.Save.SaveProviders.SaveData;
using ConnectIt.Utilities;
using System;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace ConnectIt.Audio.Sounds
{
    public class SoundsPlayer : IInitializable
    {
        public event Action VolumeChanged;

        public float VolumePercents => Mathf.Pow(10, VolumeDb / 20) * 100;
        public float VolumeDb
        {
            get
            {
                _mixer.GetFloat(MixerExposedParametersConstants.SoundsVolume, out float value);
                return value;
            }
        }

        private readonly SoundsAudioSourceMonoWrapper _audioSourceMonoWrapper;
        private readonly ISettingsSaveProvider _settingsSaveProvider;
        private readonly AudioMixer _mixer;

        public SoundsPlayer(
            SoundsAudioSourceMonoWrapper audioSourceMonoWrapper,
            ISettingsSaveProvider settingsSaveProvider,
            AudioMixer mixer)
        {
            _audioSourceMonoWrapper = audioSourceMonoWrapper;
            _settingsSaveProvider = settingsSaveProvider;
            _mixer = mixer;
        }

        public void Initialize()
        {
            SetVolumePercent(_settingsSaveProvider.LoadSettingsData().SoundsVolumePercents);
        }

        public void SetVolumeDb(float newVolume, bool save = true)
        {
            if (newVolume == VolumeDb && !save)
                return;

            Assert.ThatArgIs(newVolume <= 20, newVolume >= -80);

            _mixer.SetFloat(MixerExposedParametersConstants.SoundsVolume, newVolume);
            if (save)
            {
                SettingsSaveData saveData = _settingsSaveProvider.LoadSettingsData();
                saveData.SoundsVolumePercents = VolumePercents;
                _settingsSaveProvider.SaveSettingsData(saveData);
            }

            VolumeChanged?.Invoke();
        }

        public void SetVolumePercent(float newVolumePercent, bool save = true)
        {
            float normalizedVolume = Mathf.Clamp(newVolumePercent / 100, 0.0001f, 1);

            float volumeDb = Mathf.Log10(normalizedVolume) * 20;

            SetVolumeDb(volumeDb, save);
        }

        public void Play(AudioClip clip, SoundMixerGroup playOn)
        {
            _audioSourceMonoWrapper.Play(clip, playOn);
        }
    }
}
