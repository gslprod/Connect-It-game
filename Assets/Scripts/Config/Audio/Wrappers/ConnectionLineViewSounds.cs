using System;
using UnityEngine;

namespace ConnectIt.Config.Wrappers
{
    [Serializable]
    public class ConnectionLineViewSounds
    {
        public AudioClip Expanding => _expanding;
        public AudioClip Completing => _completing;
        public AudioClip Creating => _creating;
        public AudioClip Destroying => _destroying;

        [SerializeField] private AudioClip _expanding;
        [SerializeField] private AudioClip _completing;
        [SerializeField] private AudioClip _creating;
        [SerializeField] private AudioClip _destroying;
    }
}
