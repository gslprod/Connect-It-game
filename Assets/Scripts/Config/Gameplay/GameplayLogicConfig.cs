using UnityEngine;

namespace ConnectIt.Config
{
    [CreateAssetMenu(fileName = "GameplayLogicConfig.asset", menuName = "Config/GameplayLogicConfig")]
    public class GameplayLogicConfig : ScriptableObject
    {
        public float RemoveConnectionLineHoldDurationSec => _removeConnectionLineHoldDurationSec;

        [Tooltip("Remove Connection Line Hold Duration Sec")]
        [SerializeField] private float _removeConnectionLineHoldDurationSec = 1f;
    }
}
