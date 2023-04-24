using System;

namespace ConnectIt.Gameplay.Data
{
    public interface ILevelsPassDataProvider
    {
        event Action LevelDataChanged;

        LevelData GetDataByLevelId(int levelId);
        void SaveData(LevelData dataToSave);
        bool TryGetDataByLevelId(int levelId, out LevelData levelData);
    }
}