using ConnectIt.Config.ScriptableObjects;
using ConnectIt.Config.Wrappers;
using ConnectIt.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectIt.Config
{
    public class GameplayViewConfig
    {
        private List<ColorByCompatibilityIndexSet> _colorsByIndeces;

        private readonly GameplayViewConfigSO _configSO;

        public GameplayViewConfig(GameplayViewConfigSO configSO)
        {
            _configSO = configSO;

            SetValuesFromSO();
        }

        public Color GetColorByCompatibilityIndex(int compatibilityIndex)
        {
            int foundIndex = _colorsByIndeces.FindIndex(set => set.CompatibilityIndex == compatibilityIndex);
            Assert.That(foundIndex >= 0);

            return _colorsByIndeces[foundIndex].Color;
        }

        private void SetValuesFromSO()
        {
            _colorsByIndeces = _configSO.ColorsByIndeces;
        }
    }
}
