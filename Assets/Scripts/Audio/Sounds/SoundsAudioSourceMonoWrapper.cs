using ConnectIt.Utilities;
using UnityEngine;

namespace ConnectIt.Audio.Sounds
{
    public class SoundsAudioSourceMonoWrapper : MonoBehaviour
    {
        [SerializeField] private AudioSource _uiAudioSource;
        [SerializeField] private AudioSource _gameplayAudioSource;

        private void Awake()
        {
            //todo
            //for some reason on android Zenject doesnt marks this object as DontDestroyOnLoad, whether this object is creating from ProjectContext
            //in the editor playmode this problem doesnt appear
            DontDestroyOnLoad(gameObject);
        }

        public void Play(AudioClip clip, SoundMixerGroup playOn)
        {
            Assert.ArgIsNotNull(clip);

            GetAudioSourceByGroup(playOn).PlayOneShot(clip);
        }

        private AudioSource GetAudioSourceByGroup(SoundMixerGroup group)
            => group switch
            {
                SoundMixerGroup.UI => _uiAudioSource,
                SoundMixerGroup.Gameplay => _gameplayAudioSource,

                _ => throw Assert.GetFailException()
            };
    }
}
