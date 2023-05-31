using ConnectIt.Config;
using ConnectIt.Utilities;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace ConnectIt.Audio.OST
{
    public class OSTAudioSourceMonoWrapper : MonoBehaviour
    {
        public event Action<AudioClip> PlayingEnded;
        public bool Playing => _audioSource.isPlaying;

        private AudioSource _audioSource;
        private AudioConfig _audioConfig;
        private float _fadeDurationSec => _audioConfig.FadeDurationSec;
        private AnimationCurve _fadeCurve => _audioConfig.FadeCurve;

        private Coroutine _waitForClipEndCoroutine;

        private void Awake()
        {
            //todo
            //for some reason on android Zenject doesnt marks this object as DontDestroyOnLoad, whether this object is creating from ProjectContext
            //in the editor playmode this problem doesnt appear
            DontDestroyOnLoad(gameObject);

            _audioSource = GetComponent<AudioSource>();
        }

        [Inject]
        public void Constructor(
            AudioConfig audioConfig)
        {
            _audioConfig = audioConfig;
        }

        public void Play(AudioClip clip, bool loop = false)
        {
            Assert.ArgIsNotNull(clip);

            if (_audioSource.isPlaying)
                PlayNext(clip, loop);
            else
                PlayNow(clip, loop);
        }

        public void Stop()
        {
            Stop(false);
        }

        private void Stop(bool onlyFade)
        {
            Tween tween = _audioSource.DOFade(0f, _fadeDurationSec)
                                      .SetEase(_fadeCurve);
            if (!onlyFade)
                tween.OnComplete(OnStopFadeCompleted);

            if (_audioSource.loop)
                _audioSource.loop = false;
            else
                StopCoroutine(_waitForClipEndCoroutine);
        }

        private void OnStopFadeCompleted()
        {
            _audioSource.Stop();
        }

        private void PlayNow(AudioClip clip, bool loop)
        {
            _audioSource.clip = clip;
            _audioSource.loop = loop;
            _audioSource.volume = 0f;
            _audioSource.DOFade(1f, _fadeDurationSec)
                        .SetEase(_fadeCurve);

            _audioSource.Play();
            if (!loop)
                _waitForClipEndCoroutine = StartCoroutine(WaitForClipEnd(clip));
        }

        private void PlayNext(AudioClip clip, bool loop)
        {
            Stop(true);

            _audioSource.loop = loop;
            _audioSource.DOFade(1f, _fadeDurationSec)
                        .SetDelay(_fadeDurationSec)
                        .SetEase(_fadeCurve)
                        .OnStart(() => OnPreviousClipFaded(clip));
            
            if (!loop)
                _waitForClipEndCoroutine = StartCoroutine(WaitForClipEnd(clip, true));
        }

        private void OnPreviousClipFaded(AudioClip newClip)
        {
            _audioSource.clip = newClip;
            _audioSource.Play();
        }

        private IEnumerator WaitForClipEnd(AudioClip clip, bool delayed = false)
        {
            yield return new WaitForSeconds(clip.length + (delayed ? _fadeDurationSec : 0));

            PlayingEnded?.Invoke(clip);
        }
    }
}
