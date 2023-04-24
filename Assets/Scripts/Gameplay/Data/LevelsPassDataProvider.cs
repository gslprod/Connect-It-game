using ConnectIt.Config;
using ConnectIt.Save.SaveProviders;
using ConnectIt.Save.SaveProviders.SaveData;
using ConnectIt.Utilities;
using ConnectIt.Utilities.Extensions;
using System;
using System.Linq;
using Zenject;
using static ConnectIt.Save.SaveProviders.SaveData.GameplaySaveData;

namespace ConnectIt.Gameplay.Data
{
    public class LevelsPassDataProvider : ILevelsPassDataProvider, IInitializable, IDisposable
    {
        public event Action LevelDataChanged;

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

            CreateLevelData();
            SaveDataArray();

            _gameplaySaveProvider.GameplaySaveDataChanged += OnGameplaySaveDataChanged;
        }

        public void Dispose()
        {
            _gameplaySaveProvider.GameplaySaveDataChanged -= OnGameplaySaveDataChanged;
        }

        public void SaveData(LevelData dataToSave)
        {
            ValidateData(dataToSave);

            int index = _levelDataArray.FindIndex(element => element.Id == dataToSave.Id);
            LevelData savedData = _levelDataArray[index];

            if (savedData.Passed && !dataToSave.Passed)
                return;

            savedData.PassState = dataToSave.PassState;

            if (dataToSave.Score >= savedData.Score)
            {
                savedData.Score = dataToSave.Score;
                savedData.PassTimeSec = dataToSave.PassTimeSec;
            }

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

        private void OnGameplaySaveDataChanged()
        {
            TryLoadLevelData();

            LevelDataChanged?.Invoke();
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
                    PassTimeSec = levelData.PassTimeSec
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
                    PassTimeSec = levelSaveData.PassTimeSec
                };

                _levelDataArray[i] = levelData;
            }

            return true;
        }

        private void CreateLevelData()
        {
            _levelDataArray = new LevelData[_gameplayLogicConfig.MaxAvailableLevel];

            for (int i = 0; i < _levelDataArray.Length; i++)
                _levelDataArray[i] = new LevelData(i + 1);
        }

        private void ValidateData(LevelData data)
        {
            Assert.ThatArgIs(data.Id > 0);
            Assert.ThatArgIs(data.PassTimeSec >= 0);
            Assert.ThatArgIs(data.Score >= 0);
        }
    }
}
