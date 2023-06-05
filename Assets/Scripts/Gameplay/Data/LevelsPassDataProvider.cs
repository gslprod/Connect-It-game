using ConnectIt.Config;
using ConnectIt.Save.SaveProviders;
using ConnectIt.Save.SaveProviders.SaveData;
using ConnectIt.Utilities;
using ConnectIt.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;
using static ConnectIt.Save.SaveProviders.SaveData.GameplaySaveData;

namespace ConnectIt.Gameplay.Data
{
    public class LevelsPassDataProvider : ILevelsPassDataProvider, IInitializable
    {
        public event Action LevelDataChanged;

        public IEnumerable<LevelData> LevelDataArray => _levelDataArray;

        private readonly GameplayLogicConfig _gameplayLogicConfig;
        private readonly IGameplaySaveProvider _gameplaySaveProvider;

        private LevelData[] _levelDataArray;

        public LevelsPassDataProvider(
            GameplayLogicConfig gameplayLogicConfig,
            IGameplaySaveProvider gameplaySaveProvider)
        {
            _gameplayLogicConfig = gameplayLogicConfig;
            _gameplaySaveProvider = gameplaySaveProvider;
        }

        public void Initialize()
        {
            if (TryLoadLevelData())
                return;

            CreateMissingLevelData();
            SaveDataArray();
        }

        public void SaveData(LevelData dataToSave)
        {
            ValidateData(dataToSave);

            int index = _levelDataArray.FindIndex(element => element.Id == dataToSave.Id);
            LevelData savedData = _levelDataArray[index];

            if (savedData.Passed && !dataToSave.Passed)
                return;

            if (savedData.PassedWithoutBoosts && !dataToSave.PassedWithoutBoosts)
                return;

            savedData.PassState = dataToSave.PassState;

            if (dataToSave.PassLevelProgress >= savedData.PassLevelProgress)
            {
                if (dataToSave.PassLevelProgress > savedData.PassLevelProgress ||
                    savedData.PassTimeSec == 0f ||
                    dataToSave.PassTimeSec < savedData.PassTimeSec)

                    savedData.PassTimeSec = dataToSave.PassTimeSec;

                if (dataToSave.PassLevelProgress > savedData.PassLevelProgress ||
                    dataToSave.Score > savedData.Score)

                    savedData.Score = dataToSave.Score;

                if (dataToSave.PassLevelProgress > savedData.PassLevelProgress)
                {
                    savedData.BoostsUsed = dataToSave.BoostsUsed;
                    savedData.PassLevelProgress = dataToSave.PassLevelProgress;
                }
            }

            if (dataToSave.TotalEarnedCoins > savedData.TotalEarnedCoins)
                savedData.TotalEarnedCoins = dataToSave.TotalEarnedCoins;

            _levelDataArray[index] = savedData;

            SaveDataArray();
            LevelDataChanged?.Invoke();
        }

        public LevelData GetDataByLevelId(int levelId)
        {
            return _levelDataArray.First(element => element.Id == levelId);
        }

        public bool TryGetDataByLevelId(int levelId, out LevelData levelData)
        {
            levelData = default;
            int index = _levelDataArray.FindIndex(element => element.Id == levelId);

            if (index < 0)
                return false;

            levelData = _levelDataArray[index];
            return true;
        }

        private void SaveDataArray()
        {
            LevelPassSaveData[] levelPassSaveData = new LevelPassSaveData[_levelDataArray.Length];

            for (int i = 0; i < _levelDataArray.Length; i++)
            {
                LevelData levelData = _levelDataArray[i];

                LevelPassSaveData saveData = new()
                {
                    Id = levelData.Id,
                    PassState = levelData.PassState,
                    Score = levelData.Score,
                    TotalEarnedCoins = levelData.TotalEarnedCoins,
                    PassTimeSec = levelData.PassTimeSec,
                    PassLevelProgress = levelData.PassLevelProgress,
                    BoostsUsed = levelData.BoostsUsed
                };

                levelPassSaveData[i] = saveData;
            }

            GameplaySaveData gameplaySaveData = _gameplaySaveProvider.LoadGameplayData();
            gameplaySaveData.LevelPasses = levelPassSaveData;
            _gameplaySaveProvider.SaveGameplayData(gameplaySaveData);
        }

        private bool TryLoadLevelData()
        {
            GameplaySaveData gameplaySaveData = _gameplaySaveProvider.LoadGameplayData();

            if (gameplaySaveData.LevelPasses == null)
                return false;

            _levelDataArray = new LevelData[gameplaySaveData.LevelPasses.Length];
            for (int i = 0; i < gameplaySaveData.LevelPasses.Length; i++)
            {
                LevelPassSaveData levelSaveData = gameplaySaveData.LevelPasses[i];

                LevelData levelData = new(levelSaveData.Id)
                {
                    PassState = levelSaveData.PassState,
                    Score = levelSaveData.Score,
                    TotalEarnedCoins = levelSaveData.TotalEarnedCoins,
                    PassTimeSec = levelSaveData.PassTimeSec,
                    PassLevelProgress = levelSaveData.PassLevelProgress,
                    BoostsUsed = levelSaveData.BoostsUsed
                };

                _levelDataArray[i] = levelData;
            }

            if (_levelDataArray.Length < _gameplayLogicConfig.MaxAvailableLevel)
                CreateMissingLevelData();

            return true;
        }

        private void CreateMissingLevelData()
        {
            LevelData[] oldArray = _levelDataArray;
            _levelDataArray = new LevelData[_gameplayLogicConfig.MaxAvailableLevel];

            for (int i = 0; i < _levelDataArray.Length; i++)
            {
                bool oldElementExists = oldArray != null && i < oldArray.Length;

                _levelDataArray[i] = oldElementExists ? oldArray[i] : new LevelData(i + 1);
            }

            SaveDataArray();
        }

        private void ValidateData(LevelData data)
        {
            Assert.ThatArgIs(data.Id > 0);
            Assert.ThatArgIs(data.PassTimeSec >= 0);
            Assert.ThatArgIs(data.Score >= 0);
            Assert.ThatArgIs(data.TotalEarnedCoins >= 0);
            Assert.ThatArgIs(data.PassLevelProgress >= 0, data.PassLevelProgress <= 100);
        }
    }
}
