using System;
using UnityEngine;

namespace ConnectIt.Config.Wrappers
{
    [Serializable]
    public class ConnectionLineViewSounds
    {
        public AudioClip Expanding => _expanding;
        public AudioClip Completing => _completing;

        [SerializeField] private AudioClip _expanding;
        [SerializeField] private AudioClip _completing;
    }
}
