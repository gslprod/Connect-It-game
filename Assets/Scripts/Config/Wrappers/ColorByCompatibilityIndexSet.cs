using System;
using UnityEngine;

namespace ConnectIt.Config.Wrappers
{
    [Serializable]
    public class ColorByCompatibilityIndexSet
    {
        public int CompatibilityIndex => _compatibilityIndex;
        public Color Color  => _color;

        [SerializeField] private int _compatibilityIndex;
        [SerializeField] private Color _color;
    }
}
