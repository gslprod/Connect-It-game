using Zenject;

namespace ConnectIt.Shop.Goods.Boosts.UsageContext
{
    public class BoostUsageContext
    {
        public CommonUsageData CommonData { get; }

        public BoostUsageContext(CommonUsageData commonData)
        {
            CommonData = commonData;
        }

        public class Factory : PlaceholderFactory<CommonUsageData, BoostUsageContext> { }
    }
}
