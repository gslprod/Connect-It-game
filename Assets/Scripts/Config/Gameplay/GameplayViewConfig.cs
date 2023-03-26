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
        [SerializeField] private List<ColorByCompatibilityIndexSet> ColorsByIndeces;

        private void OnEnable()
        {
            ValidateColorsByIndeces();
        }

        public Color GetColorByCompatibilityIndex(int compatibilityIndex)
        {
            int foundIndex = ColorsByIndeces.FindIndex(set => set.CompatibilityIndex == compatibilityIndex);
            Assert.That(foundIndex >= 0);

            return ColorsByIndeces[foundIndex].Color;
        }

        private void ValidateColorsByIndeces()
        {
            if (ColorsByIndeces == null)
                return;

            var groupsWithDuplicateIndecesCount =
                            ColorsByIndeces.GroupBy(set => set.CompatibilityIndex)
                            .Where(group => group.Count() > 1)
                            .Count();

            Assert.That(groupsWithDuplicateIndecesCount == 0);
        }
    }
}
