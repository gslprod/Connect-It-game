using System;
using UnityEngine;

namespace ConnectIt.Config.Wrappers
{
    [Serializable]
    public class LevelRewardData
    {
        public int Level => _level;
        public RewardByTime[] RewardsByTime => _rewardsByTime;
        public RewardByMoves[] RewardsByMoves => _rewardsByMoves;
        public long ScoresBaseReward => _scoresBaseReward;
        public long CoinsBaseReward => _coinsBaseReward;
        public bool CoinsDependsOnScores => _coinsDependsOnScores;

        [Min(1)] [SerializeField] private int _level;
        [SerializeField] private RewardByTime[] _rewardsByTime;
        [SerializeField] private RewardByMoves[] _rewardsByMoves;
        [Min(0)] [SerializeField] private long _scoresBaseReward;
        [Min(0)] [SerializeField] private long _coinsBaseReward;
        [SerializeField] private bool _coinsDependsOnScores;

        [Serializable]
        public class RewardByTime
        {
            public float TimeKeySec => _timeKeySec;
            public float ScoresRewardMultiplier => _scoresRewardMultiplier;
            public float CoinsRewardMultiplier => _coinsRewardMultiplier;

            [Min(0)] [SerializeField] private float _timeKeySec;
            [Min(0)] [SerializeField] private float _scoresRewardMultiplier;
            [Min(0)] [SerializeField] private float _coinsRewardMultiplier;
        }

        [Serializable]
        public class RewardByMoves
        {
            public int MoveCountKey => _moveCountKey;
            public float ScoresRewardMultiplier => _scoresRewardMultiplier;
            public float CoinsRewardMultiplier => _coinsRewardMultiplier;

            [Min(0)] [SerializeField] private int _moveCountKey;
            [Min(0)] [SerializeField] private float _scoresRewardMultiplier;
            [Min(0)] [SerializeField] private float _coinsRewardMultiplier;
        }
    }
}
