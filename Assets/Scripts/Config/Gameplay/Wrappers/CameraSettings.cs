using System;
using UnityEngine;

namespace ConnectIt.Config.Wrappers
{
    [Serializable]
    public class CameraSettings
    {
        public float SizePadding => _sizePadding;
        public float MinSize => _minSize;
        public float MaxSize => _maxSize;

        [SerializeField] private float _sizePadding;
        [SerializeField] private float _minSize;
        [SerializeField] private float _maxSize;
    }
}
