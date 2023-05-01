using UnityEngine;

namespace ConnectIt.Config.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ShopConfig.asset", menuName = "Config/ShopConfig")]
    public class ShopConfigSO : ScriptableObject
    {
        public long SkipLevelBoostPrice => _skipLevelBoostPrice;

        [Tooltip("Skip Level Boost Price")]
        [SerializeField] private long _skipLevelBoostPrice;
    }
}
