using UnityEngine;

namespace ConnectIt.Config.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ShopConfig.asset", menuName = "Config/ShopConfig")]
    public class ShopConfigSO : ScriptableObject
    {
        public long SkipLevelBoostPrice => _skipLevelBoostPrice;
        public long SimplifyLevelBoostPrice => _simplifyLevelBoostPrice;
        public long AllowIncompatibleConnectionsBoostPrice => _allowIncompatibleConnectionsPrice;

        [Tooltip("Skip Level Boost Price")]
        [SerializeField] private long _skipLevelBoostPrice;

        [Tooltip("Simplify Level Boost Price")]
        [SerializeField] private long _simplifyLevelBoostPrice;

        [Tooltip("Allow Incompatible Connections Boost Price")]
        [SerializeField] private long _allowIncompatibleConnectionsPrice;
    }
}
