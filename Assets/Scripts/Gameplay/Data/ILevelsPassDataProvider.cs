using System;
using System.Collections.Generic;

namespace ConnectIt.Gameplay.Data
{
    public interface ILevelsPassDataProvider
    {
        IEnumerable<LevelData> LevelDataArray { get; }

        event Action LevelDataChanged;

        LevelData GetDataByLevelId(int levelId);
        void SaveData(LevelData dataToSave);
        bool TryGetDataByLevelId(int levelId, out LevelData levelData);
    }
}