using ConnectIt.Gameplay.Data;
using ConnectIt.Stats.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace ConnectIt.Stats.Modules
{
    public class LevelsStatsModule : IStatsModule, IInitializable, IDisposable
    {
        private readonly IStatsCenter _statsCenter;
        private readonly ILevelsPassDataProvider _levelsPassDataProvider;

        private PassedLevelsScoreSumStatsData _passedLevelsScoreSumStatsData;
        private PassedWithoutBoostsLevelsScoreSumStatsData _passedWithoutBoostsLevelsScoreSumStatsData;
        private PassedLevelsTimeSumStatsData _passedLevelsTimeSumStatsData;
        private PassedWithoutBoostsLevelsTimeSumStatsData _passedWithoutBoostsLevelsTimeSumStatsData;

        public LevelsStatsModule(
            IStatsCenter statsCenter,
            ILevelsPassDataProvider levelsPassDataProvider)
        {
            _statsCenter = statsCenter;
            _levelsPassDataProvider = levelsPassDataProvider;
        }

        public void Initialize()
        {
            _passedLevelsScoreSumStatsData = _statsCenter.GetData<PassedLevelsScoreSumStatsData>();
            _passedWithoutBoostsLevelsScoreSumStatsData = _statsCenter.GetData<PassedWithoutBoostsLevelsScoreSumStatsData>();
            _passedLevelsTimeSumStatsData = _statsCenter.GetData<PassedLevelsTimeSumStatsData>();
            _passedWithoutBoostsLevelsTimeSumStatsData = _statsCenter.GetData<PassedWithoutBoostsLevelsTimeSumStatsData>();

            _statsCenter.RegisterModule(this);

            if (_levelsPassDataProvider.LevelDataArray.Any(item => item.Passed) &&
                (_passedLevelsScoreSumStatsData.RawValue == 0 || 
                _passedWithoutBoostsLevelsScoreSumStatsData.RawValue == 0 ||
                _passedLevelsTimeSumStatsData.RawValue == 0 ||
                _passedWithoutBoostsLevelsTimeSumStatsData.RawValue == 0))
            {
                UpdateStatsData();
            }

            _levelsPassDataProvider.LevelDataChanged += OnLevelDataChanged;
        }

        public void Dispose()
        {
            _statsCenter.UnregisterModule(this);

            _levelsPassDataProvider.LevelDataChanged -= OnLevelDataChanged;
        }

        private void UpdateStatsData()
        {
            IEnumerable<LevelData> levelData = _levelsPassDataProvider.LevelDataArray;

            _passedLevelsScoreSumStatsData.RawValue = levelData.Sum(item => item.Score);
            _passedWithoutBoostsLevelsScoreSumStatsData.RawValue = levelData.Sum(item => item.FullyPassedWithoutBoosts ? item.Score : 0);
            _passedLevelsTimeSumStatsData.RawValue = levelData.Sum(item => (double)item.PassTimeSec);
            _passedWithoutBoostsLevelsTimeSumStatsData.RawValue = levelData.Sum(item => item.FullyPassedWithoutBoosts ? (double)item.PassTimeSec : 0);
        }

        private void OnLevelDataChanged()
        {
            UpdateStatsData();
        }
    }
}
