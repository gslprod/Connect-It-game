using ConnectIt.Config.ScriptableObjects;
using ConnectIt.Config.Wrappers;
using ConnectIt.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectIt.Config
{
    public class GameplayViewConfig
    {
        public CameraSettings CameraSettings => _configSO.CameraSettings;

        private List<ColorByCompatibilityIndexSet> _colorsByIndeces => _configSO.ColorsByIndeces;

        private readonly GameplayViewConfigSO _configSO;

        public GameplayViewConfig(GameplayViewConfigSO configSO)
        {
            _configSO = configSO;
        }

        public Color GetColorByCompatibilityIndex(int compatibilityIndex)
        {
            int foundIndex = _colorsByIndeces.FindIndex(set => set.CompatibilityIndex == compatibilityIndex);
            Assert.That(foundIndex >= 0);

            return _colorsByIndeces[foundIndex].Color;
        }
    }
}
