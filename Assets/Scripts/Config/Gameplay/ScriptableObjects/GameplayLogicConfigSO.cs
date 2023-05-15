using ConnectIt.Config.Wrappers;
using UnityEngine;

namespace ConnectIt.Config.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameplayLogicConfig.asset", menuName = "Config/GameplayLogicConfig")]
    public class GameplayLogicConfigSO : ScriptableObject
    {
        public float RemoveConnectionLineHoldDurationSec => _removeConnectionLineHoldDurationSec;
        public float UsedTilesVsConnectedPortsGameCompleteFactor => _usedTilesVsConnectedPortsGameCompleteFactor;
        public int MaxAvailableLevel => _maxAvailableLevel;
        public LevelRewardData[] LevelRewardData => _levelsRewardData;

        [Tooltip("Remove Connection Line Hold Duration Sec")]
        [SerializeField] private float _removeConnectionLineHoldDurationSec = 1f;

        [Tooltip("Used Tiles Vs Connected Ports Game Complete Factor. Dominations: 1 -> Used Tiles, 0 -> Connected ports")]
        [Range(0f, 1f)]
        [SerializeField] private float _usedTilesVsConnectedPortsGameCompleteFactor;

        [Min(1)]
        [SerializeField] private int _maxAvailableLevel = 1;

        [SerializeField] private LevelRewardData[] _levelsRewardData;
    }
}
