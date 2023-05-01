using ConnectIt.Config.ScriptableObjects;

namespace ConnectIt.Config
{
    public class ShopConfig
    {
        public long SkipLevelBoostPrice => _configSO.SkipLevelBoostPrice;

        private readonly ShopConfigSO _configSO;

        public ShopConfig(ShopConfigSO configSO)
        {
            _configSO = configSO;
        }
    }
}
