using ConnectIt.Config.ScriptableObjects;

namespace ConnectIt.Config
{
    public class ShopConfig
    {
        public long SkipLevelBoostPrice => _configSO.SkipLevelBoostPrice;
        public long SimplifyLevelBoostPrice => _configSO.SimplifyLevelBoostPrice;
        public long AllowIncompatibleConnectionsBoostPrice => _configSO.AllowIncompatibleConnectionsBoostPrice;

        private readonly ShopConfigSO _configSO;

        public ShopConfig(ShopConfigSO configSO)
        {
            _configSO = configSO;
        }
    }
}
