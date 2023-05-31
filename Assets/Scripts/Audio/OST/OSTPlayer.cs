using ConnectIt.Config;
using ConnectIt.Config.Wrappers;
using ConnectIt.Save.SaveProviders;
using ConnectIt.Save.SaveProviders.SaveData;
using ConnectIt.Scenes;
using ConnectIt.Utilities;
using ConnectIt.Utilities.Extensions;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace ConnectIt.Audio.OST
{
    public class OSTPlayer : IInitializable, IDisposable
    {
        public event Action VolumeChanged;

        public float VolumePercents => Mathf.InverseLerp(-80, 20, Volume) * 100;
        public float Volume
        {
            get
            {
                _mixer.GetFloat(MixerExposedParametersConstants.OSTVolume, out float value);
                return value;
            }
        }

        private readonly OSTAudioSourceMonoWrapper _audioSourceMonoWrapper;
        private readonly AudioConfig _audioConfig;
        private readonly IScenesLoader _scenesLoader;
        private readonly ISettingsSaveProvider _settingsSaveProvider;
        private readonly AudioMixer _mixer;

        private SceneOSTPlayInfo _current;
        private int _playingClipIndex;

        public OSTPlayer(
            OSTAudioSourceMonoWrapper audioSourceMonoWrapper,
            AudioConfig audioConfig,
            IScenesLoader scenesLoader,
            ISettingsSaveProvider settingsSaveProvider,
            AudioMixer mixer)
        {
            _audioSourceMonoWrapper = audioSourceMonoWrapper;
            _audioConfig = audioConfig;
            _scenesLoader = scenesLoader;
            _settingsSaveProvider = settingsSaveProvider;
            _mixer = mixer;
        }

        public void Initialize()
        {
            _scenesLoader.OnNewSceneAsyncLoadStarted += OnNewSceneAsyncLoadStarted;
            _audioSourceMonoWrapper.PlayingEnded += OnPlayingEnded;

            SetVolumePercent(_settingsSaveProvider.LoadSettingsData().OSTVolumePercents);
            TryStartPlayingOSTInScene(_scenesLoader.ActiveScene);
        }

        public void Dispose()
        {
            _scenesLoader.OnNewSceneAsyncLoadStarted -= OnNewSceneAsyncLoadStarted;
            _audioSourceMonoWrapper.PlayingEnded -= OnPlayingEnded;
        }

        public void SetVolume(float newVolume, bool save = true)
        {
            if (newVolume == Volume && !save)
                return;

            Assert.ThatArgIs(newVolume <= 20, newVolume >= -80);

            _mixer.SetFloat(MixerExposedParametersConstants.OSTVolume, newVolume);
            if (save)
            {
                SettingsSaveData saveData = _settingsSaveProvider.LoadSettingsData();
                saveData.OSTVolumePercents = VolumePercents;
                _settingsSaveProvider.SaveSettingsData(saveData);
            }

            VolumeChanged?.Invoke();
        }

        public void SetVolumePercent(float newVolumePercent, bool save = true)
        {
            float volume = Mathf.Lerp(-80, 20, newVolumePercent / 100);

            SetVolume(volume, save);
        }

        private bool TryStartPlayingOSTInScene(SceneType type)
        {
            int index = _audioConfig.OSTList.FindIndex(item => item.TargetScene == type);
            if (index < 0)
                return false;

            SceneOSTPlayInfo playInfo = _audioConfig.OSTList[index];
            if (playInfo.PlayingClips.Length == 0)
                return false;

            _current = playInfo;
            _playingClipIndex = -1;

            PlayNewOST();
            return true;
        }

        private void PlayNewOST()
        {
            if (_current.PlayingClips.Length == 1)
            {
                _audioSourceMonoWrapper.Play(_current.PlayingClips[0], true);
                return;
            }

            if (_current.RandomPlayOrder && _current.PlayingClips.Length > 2)
                SetRandomOSTIndex();
            else
                SetNextOSTIndex();

            _audioSourceMonoWrapper.Play(_current.PlayingClips[_playingClipIndex]);
        }

        private void SetRandomOSTIndex()
        {
            int random = UnityEngine.Random.Range(0, _current.PlayingClips.Count() - 2);

            _playingClipIndex = random < _playingClipIndex ? random : random + 1;
        }

        private void SetNextOSTIndex()
        {
            if (_playingClipIndex == _current.PlayingClips.Length - 1)
                _playingClipIndex = 0;
            else
                _playingClipIndex++;
        }

        private void OnPlayingEnded(AudioClip obj)
        {
            PlayNewOST();
        }

        private void OnNewSceneAsyncLoadStarted(SceneType type)
        {
            if (type == _current?.TargetScene)
                return;

            if (TryStartPlayingOSTInScene(type))
                return;

            _audioSourceMonoWrapper.Stop();
            _current = null;
            _playingClipIndex = -1;
        }
    }
}
