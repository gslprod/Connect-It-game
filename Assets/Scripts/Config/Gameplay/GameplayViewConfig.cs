using ConnectIt.Config.Wrappers;
using ConnectIt.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ConnectIt.Config
{
    [CreateAssetMenu(fileName = "GameplayViewConfig.asset", menuName = "Config/GameplayViewConfig")]
    public class GameplayViewConfig : ScriptableObject
    {
        public string LevelCompleteProgressTitleFormat => _levelCompleteProgressTitleFormat;

        [SerializeField] private List<ColorByCompatibilityIndexSet> _colorsByIndeces;

        [Tooltip("Level Complete Progress Title Format")]
        [SerializeField] private string _levelCompleteProgressTitleFormat;

        private void OnEnable()
        {
            ValidateColorsByIndeces();
        }

        public Color GetColorByCompatibilityIndex(int compatibilityIndex)
        {
            int foundIndex = _colorsByIndeces.FindIndex(set => set.CompatibilityIndex == compatibilityIndex);
            Assert.That(foundIndex >= 0);

            return _colorsByIndeces[foundIndex].Color;
        }

        private void ValidateColorsByIndeces()
        {
            if (_colorsByIndeces == null)
                return;

            var groupsWithDuplicateIndecesCount =
                            _colorsByIndeces.GroupBy(set => set.CompatibilityIndex)
                            .Where(group => group.Count() > 1)
                            .Count();

            Assert.That(groupsWithDuplicateIndecesCount == 0);
        }
    }
}
