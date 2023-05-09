using ConnectIt.Shop.Goods.Boosts;
using ConnectIt.Shop.Goods.Boosts.UsageContext;
using System;
using Zenject;

namespace ConnectIt.Infrastructure.Factories.Concrete
{
    public class BoostUsageContextFactory : IFactory<CommonUsageData, BoostUsageContext>
    {
        private readonly BoostUsageContext.Factory _boostUsageContextFactory;
        private readonly SkipLevelBoostUsageContext.Factory _skipLevelBoostUsageContextFactory;

        public BoostUsageContextFactory(
            BoostUsageContext.Factory boostUsageContextFactory,
            SkipLevelBoostUsageContext.Factory skipLevelBoostUsageContextFactory)
        {
            _boostUsageContextFactory = boostUsageContextFactory;
            _skipLevelBoostUsageContextFactory = skipLevelBoostUsageContextFactory;
        }

        public BoostUsageContext Create(CommonUsageData param)
        {
            Type boostType = param.UsedBoost.GetType();

            if (boostType == typeof(SkipLevelBoost))
                return _skipLevelBoostUsageContextFactory.Create(param);
            
            return _boostUsageContextFactory.Create(param);
        }
    }
}
