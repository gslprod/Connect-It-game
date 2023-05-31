using ConnectIt.Scenes;
using System;
using UnityEngine;

namespace ConnectIt.Config.Wrappers
{
    [Serializable]
    public class SceneOSTPlayInfo
    {
        public SceneType TargetScene => _targetScene;
        public bool RandomPlayOrder => _randomPlayOrder;
        public AudioClip[] PlayingClips => _playingClips;

        [SerializeField] private SceneType _targetScene;
        [SerializeField] private bool _randomPlayOrder;
        [SerializeField] private AudioClip[] _playingClips;
    }
}
