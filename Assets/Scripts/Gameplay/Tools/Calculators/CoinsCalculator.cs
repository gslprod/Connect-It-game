using ConnectIt.Config.Wrappers;
using ConnectIt.Config;
using ConnectIt.Gameplay.Observers;
using ConnectIt.Gameplay.Time;
using ConnectIt.Utilities.Extensions;
using static ConnectIt.Config.Wrappers.LevelRewardData;
using System;
using ConnectIt.Utilities;
using System.Linq;
using UnityEngine;

namespace ConnectIt.Gameplay.Tools.Calculators
{
    public class CoinsCalculator : ICoinsCalculator
    {
        private readonly IGameStateObserver _gameStateObserver;
        private readonly GameplayLogicConfig _gameplayLogicConfig;
        private readonly IGameplayTimeProvider _gameplayTimeProvider;
        private readonly IScoresCalculator _scoresCalculator;

        public CoinsCalculator(
            IGameStateObserver gameStateObserver,
            GameplayLogicConfig gameplayLogicConfig,
            IGameplayTimeProvider gameplayTimeProvider,
            IScoresCalculator scoresCalculator)
        {
            _gameStateObserver = gameStateObserver;
            _gameplayLogicConfig = gameplayLogicConfig;
            _gameplayTimeProvider = gameplayTimeProvider;
            _scoresCalculator = scoresCalculator;
        }

        public long Calculate()
        {
            LevelRewardData rewardData = GetDataAndValidate();

            if (rewardData.CoinsDependsOnScores)
            {
                float result = _scoresCalculator.Calculate() * ((float)rewardData.CoinsBaseReward / rewardData.ScoresBaseReward);
                return Convert.ToInt64(result);
            }

            RewardByTime[] rewardsByTime = rewardData.RewardsByTime;
            int timeRangeStartIndex = rewardsByTime.FindIndex(
                key => key.TimeKeySec <= _gameplayTimeProvider.ElapsedTimeSec);

            RewardByMoves[] rewardsByMoves = rewardData.RewardsByMoves;
            int moveRangeStartIndex = rewardsByMoves.FindIndex(
                key => key.MoveCountKey <= _gameStateObserver.MovesCount);

            float timeMultiplier = CalculateTimeMultiplier(rewardsByTime, timeRangeStartIndex);
            float moveMultiplier = CalculateMovesMultiplier(rewardsByMoves, moveRangeStartIndex);

            return FinalCalculation(rewardData.ScoresBaseReward, timeMultiplier, moveMultiplier);
        }

        private float CalculateMovesMultiplier(RewardByMoves[] rewardsByMoves, int moveRangeStartIndex)
        {
            if (moveRangeStartIndex < 0)
            {
                return rewardsByMoves[0].CoinsRewardMultiplier;
            }
            else if (moveRangeStartIndex == rewardsByMoves.Length - 1)
            {
                return rewardsByMoves[moveRangeStartIndex].CoinsRewardMultiplier;
            }
            else
            {
                float t = Mathf.InverseLerp(
                    rewardsByMoves[moveRangeStartIndex].MoveCountKey,
                    rewardsByMoves[moveRangeStartIndex + 1].MoveCountKey,
                    _gameStateObserver.MovesCount);

                return Mathf.Lerp(
                    rewardsByMoves[moveRangeStartIndex].CoinsRewardMultiplier,
                    rewardsByMoves[moveRangeStartIndex + 1].CoinsRewardMultiplier,
                    t);
            }
        }

        private float CalculateTimeMultiplier(RewardByTime[] rewardsByTime, int timeRangeStartIndex)
        {
            if (timeRangeStartIndex < 0)
            {
                return rewardsByTime[0].CoinsRewardMultiplier;
            }
            else if (timeRangeStartIndex == rewardsByTime.Length - 1)
            {
                return rewardsByTime[timeRangeStartIndex].CoinsRewardMultiplier;
            }
            else
            {
                float t = Mathf.InverseLerp(
                    rewardsByTime[timeRangeStartIndex].TimeKeySec,
                    rewardsByTime[timeRangeStartIndex + 1].TimeKeySec,
                    _gameplayTimeProvider.ElapsedTimeSec);

                return Mathf.Lerp(
                    rewardsByTime[timeRangeStartIndex].CoinsRewardMultiplier,
                    rewardsByTime[timeRangeStartIndex + 1].CoinsRewardMultiplier,
                    t);
            }
        }

        private long FinalCalculation(long scoresBaseReward, float timeMultiplier, float movesMultiplier)
            => Convert.ToInt64(timeMultiplier * movesMultiplier * scoresBaseReward);

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

