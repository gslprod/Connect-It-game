using System;

namespace ConnectIt.Gameplay.Data
{
    public interface ILevelsPassDataProvider
    {
        event Action LevelDataChanged;

        LevelData GetDataByLevelId(int levelId);
        void SaveData(LevelData dataToSave);
    }
}