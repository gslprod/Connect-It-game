using ConnectIt.Config;
using ConnectIt.Config.Wrappers;
using ConnectIt.Gameplay.Observers;
using ConnectIt.Gameplay.Time;
using ConnectIt.Utilities;
using ConnectIt.Utilities.Extensions;
using System;
using System.Linq;
using UnityEngine;
using static ConnectIt.Config.Wrappers.LevelRewardData;

namespace ConnectIt.Gameplay.Tools.Calculators
{
    public class ScoresCalculator : IScoresCalculator
    {
        private readonly IGameStateObserver _gameStateObserver;
        private readonly GameplayLogicConfig _gameplayLogicConfig;
        private readonly IGameplayTimeProvider _gameplayTimeProvider;

        public ScoresCalculator(
            IGameStateObserver gameStateObserver,
            GameplayLogicConfig gameplayLogicConfig,
            IGameplayTimeProvider gameplayTimeProvider)
        {
            _gameStateObserver = gameStateObserver;
            _gameplayLogicConfig = gameplayLogicConfig;
            _gameplayTimeProvider = gameplayTimeProvider;
        }

        public long Calculate()
        {
            LevelRewardData rewardData = GetDataAndValidate();

            RewardByTime[] rewardsByTime = rewardData.RewardsByTime;
            int timeRangeStartIndex = rewardsByTime.FindIndexLast(
                key => key.TimeKeySec <= _gameplayTimeProvider.ElapsedTimeSec);

            RewardByMoves[] rewardsByMoves = rewardData.RewardsByMoves;
            int moveRangeStartIndex = rewardsByMoves.FindIndexLast(
                key => key.MoveCountKey <= _gameStateObserver.MovesCount);

            float timeMultiplier = CalculateTimeMultiplier(rewardsByTime, timeRangeStartIndex);
            float moveMultiplier = CalculateMovesMultiplier(rewardsByMoves, moveRangeStartIndex);

            return FinalCalculation(rewardData.ScoresBaseReward, timeMultiplier, moveMultiplier);
        }

        private float CalculateMovesMultiplier(RewardByMoves[] rewardsByMoves, int moveRangeStartIndex)
        {
            if (moveRangeStartIndex < 0)
            {
                return rewardsByMoves[0].ScoresRewardMultiplier;
            }
            else if (moveRangeStartIndex == rewardsByMoves.Length - 1)
            {
                return rewardsByMoves[moveRangeStartIndex].ScoresRewardMultiplier;
            }
            else
            {
                float t = Mathf.InverseLerp(
                    rewardsByMoves[moveRangeStartIndex].MoveCountKey,
                    rewardsByMoves[moveRangeStartIndex + 1].MoveCountKey,
                    _gameStateObserver.MovesCount);

                return Mathf.Lerp(
                    rewardsByMoves[moveRangeStartIndex].ScoresRewardMultiplier,
                    rewardsByMoves[moveRangeStartIndex + 1].ScoresRewardMultiplier,
                    t);
            }
        }

        private float CalculateTimeMultiplier(RewardByTime[] rewardsByTime, int timeRangeStartIndex)
        {
            if (timeRangeStartIndex < 0)
            {
                return rewardsByTime[0].ScoresRewardMultiplier;
            }
            else if (timeRangeStartIndex == rewardsByTime.Length - 1)
            {
                return rewardsByTime[timeRangeStartIndex].ScoresRewardMultiplier;
            }
            else
            {
                float t = Mathf.InverseLerp(
                    rewardsByTime[timeRangeStartIndex].TimeKeySec,
                    rewardsByTime[timeRangeStartIndex + 1].TimeKeySec,
                    _gameplayTimeProvider.ElapsedTimeSec);

                return Mathf.Lerp(
                    rewardsByTime[timeRangeStartIndex].ScoresRewardMultiplier,
                    rewardsByTime[timeRangeStartIndex + 1].ScoresRewardMultiplier,
                    t);
            }
        }

        private long FinalCalculation(long scoresBaseReward, float timeMultiplier, float movesMultiplier)
            => (long)Mathf.Round(timeMultiplier * movesMultiplier * scoresBaseReward * _gameStateObserver.GameCompleteProgressPercents / 100);

        private LevelRewardData GetDataAndValidate()
        {
            int groupsWithDuplicateLevelsCount =
                _gameplayLogicConfig.LevelsRewardData.GroupBy(wrapper => wrapper.Level)
                .Count(group => group.Count() > 1);

            Assert.That(groupsWithDuplicateLevelsCount == 0);

            LevelRewardData rewardData = _gameplayLogicConfig.LevelsRewardData.First(
                item => item.Level == _gameplayLogicConfig.CurrentLevel);

            RewardByTime[] rewardsByTime = rewardData.RewardsByTime;
            Assert.That(rewardsByTime.Length > 1);
            for (int i = 0; i < rewardsByTime.Length - 1; i++)
                Assert.That(rewardsByTime[i].TimeKeySec < rewardsByTime[i + 1].TimeKeySec);

            RewardByMoves[] rewardsByMoves = rewardData.RewardsByMoves;
            Assert.That(rewardsByMoves.Length > 1);
            for (int i = 0; i < rewardsByMoves.Length - 1; i++)
                Assert.That(rewardsByMoves[i].MoveCountKey < rewardsByMoves[i + 1].MoveCountKey);

            return rewardData;
        }
    }
}
