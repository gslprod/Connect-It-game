using ConnectIt.Config.ScriptableObjects;
using ConnectIt.Config.Wrappers;
using UnityEngine;

namespace ConnectIt.Config
{
    public class AudioConfig
    {
        public SceneOSTPlayInfo[] OSTList => _configSO.OSTList;
        public float FadeDurationSec => _configSO.FadeDurationSec;
        public AnimationCurve FadeCurve => _configSO.FadeCurve;
        public ConnectionLineViewSounds ConnectionLineViewSounds => _configSO.ConnectionLineViewSounds;
        public AudioClip Click => _configSO.Click;

        private readonly AudioConfigSO _configSO;

        public AudioConfig(
            AudioConfigSO configSO)
        {
            _configSO = configSO;
        }
    }
}
