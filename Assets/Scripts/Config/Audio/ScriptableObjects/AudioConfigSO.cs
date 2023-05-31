using ConnectIt.Config.Wrappers;
using UnityEngine;

namespace ConnectIt.Config.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AudioConfig.asset", menuName = "Config/AudioConfig")]
    public class AudioConfigSO : ScriptableObject
    {
        public SceneOSTPlayInfo[] OSTList => _ostList;
        public float FadeDurationSec => _fadeDurationSec;
        public AnimationCurve FadeCurve => _fadeCurve;
        public ConnectionLineViewSounds ConnectionLineViewSounds => _connectionLineViewSounds;
        public AudioClip Click => _click;

        [Header("OST")]
        [SerializeField] private SceneOSTPlayInfo[] _ostList;
        [SerializeField] private float _fadeDurationSec;
        [SerializeField] private AnimationCurve _fadeCurve;

        [Header("Gameplay Sounds")]
        [SerializeField] private ConnectionLineViewSounds _connectionLineViewSounds;

        [Header("UI Sounds")]
        [SerializeField] private AudioClip _click;
    }
}
