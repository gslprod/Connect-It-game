using UnityEngine;

namespace ConnectIt.Config
{
    [CreateAssetMenu(fileName = "GameplayLogicConfig.asset", menuName = "Config/GameplayLogicConfig")]
    public class GameplayLogicConfig : ScriptableObject
    {
        public float RemoveConnectionLineHoldDurationSec => _removeConnectionLineHoldDurationSec;
        public float UsedTilesVsConnectedPortsGameCompleteFactor => _usedTilesVsConnectedPortsGameCompleteFactor;

        [Tooltip("Remove Connection Line Hold Duration Sec")]
        [SerializeField] private float _removeConnectionLineHoldDurationSec = 1f;

        [Tooltip("Used Tiles Vs Connected Ports Game Complete Factor. Dominations: 1 -> Used Tiles, 0 -> Connected ports")]
        [Range(0f, 1f)]
        [SerializeField] private float _usedTilesVsConnectedPortsGameCompleteFactor;
    }
}
