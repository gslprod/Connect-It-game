using ConnectIt.Config.Wrappers;
using ConnectIt.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ConnectIt.Config.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameplayViewConfig.asset", menuName = "Config/GameplayViewConfig")]
    public class GameplayViewConfigSO : ScriptableObject
    {
        public List<ColorByCompatibilityIndexSet> ColorsByIndeces => _colorsByIndeces;
        public CameraSettings CameraSettings => _cameraSettings;

        [SerializeField] private List<ColorByCompatibilityIndexSet> _colorsByIndeces;
        [SerializeField] private CameraSettings _cameraSettings;

        private void OnEnable()
        {
            ValidateColorsByIndeces();
        }

        private void ValidateColorsByIndeces()
        {
            if (_colorsByIndeces == null)
                return;

            var groupsWithDuplicateIndecesCount =
                _colorsByIndeces.GroupBy(set => set.CompatibilityIndex)
                .Count(group => group.Count() > 1);

            Assert.That(groupsWithDuplicateIndecesCount == 0);
        }
    }
}
