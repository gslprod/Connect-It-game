using ConnectIt.Gameplay.Data;
using System.Linq;

namespace ConnectIt.Utilities.Extensions
{
    public static class ILevelsPassDataProviderExtensions
    {
        public static bool TryGetLastCompletedLevelData(this ILevelsPassDataProvider source, out LevelData levelData)
        {
            levelData = default;

            int index = source.LevelDataArray.FindIndex(element => element.NotCompleted);
            if (index == 0)
                return false;

            levelData = index < 0 ? source.LevelDataArray.Last() : source.LevelDataArray.ElementAt(index - 1);
            return true;
        }
    }
}
