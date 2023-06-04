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
        private readonly SimplifyLevelBoostUsageContext.Factory _simplifyLevelBoostUsageContextFactory;

        public BoostUsageContextFactory(
            BoostUsageContext.Factory boostUsageContextFactory,
            SkipLevelBoostUsageContext.Factory skipLevelBoostUsageContextFactory,
            SimplifyLevelBoostUsageContext.Factory simplifyLevelBoostUsageContextFactory)
        {
            _boostUsageContextFactory = boostUsageContextFactory;
            _skipLevelBoostUsageContextFactory = skipLevelBoostUsageContextFactory;
            _simplifyLevelBoostUsageContextFactory = simplifyLevelBoostUsageContextFactory;
        }

        public BoostUsageContext Create(CommonUsageData param)
        {
            Type boostType = param.UsedBoost.GetType();

            if (boostType == typeof(SkipLevelBoost))
                return _skipLevelBoostUsageContextFactory.Create(param);

            if (boostType == typeof(SimplifyLevelBoost))
                return _simplifyLevelBoostUsageContextFactory.Create(param);

            return _boostUsageContextFactory.Create(param);
        }
    }
}
