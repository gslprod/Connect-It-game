using ConnectIt.Infrastructure.CreatedObjectNotifiers;
using ConnectIt.Shop.Goods.Boosts;
using ConnectIt.Shop.Goods.Boosts.UsageContext;
using System;
using Zenject;

namespace ConnectIt.Infrastructure.Factories.Concrete
{
    public class BoostUsageContextFactory : IFactory<CommonUsageData, BoostUsageContext>
    {
        private readonly ICreatedObjectNotifier<BoostUsageContext> _createdBoostUsageContextNotifier;
        private readonly BoostUsageContext.Factory _boostUsageContextFactory;
        private readonly SkipLevelBoostUsageContext.Factory _skipLevelBoostUsageContextFactory;
        private readonly SimplifyLevelBoostUsageContext.Factory _simplifyLevelBoostUsageContextFactory;
        private readonly AllowIncompatibleConnectionsBoostUsageContext.Factory _allowIncompatibleConnectionsBoostUsageContextFactory;

        public BoostUsageContextFactory(
            ICreatedObjectNotifier<BoostUsageContext> createdBoostUsageContextNotifier,
            BoostUsageContext.Factory boostUsageContextFactory,
            SkipLevelBoostUsageContext.Factory skipLevelBoostUsageContextFactory,
            SimplifyLevelBoostUsageContext.Factory simplifyLevelBoostUsageContextFactory,
            AllowIncompatibleConnectionsBoostUsageContext.Factory allowIncompatibleConnectionsBoostUsageContextFactory)
        {
            _createdBoostUsageContextNotifier = createdBoostUsageContextNotifier;
            _boostUsageContextFactory = boostUsageContextFactory;
            _skipLevelBoostUsageContextFactory = skipLevelBoostUsageContextFactory;
            _simplifyLevelBoostUsageContextFactory = simplifyLevelBoostUsageContextFactory;
            _allowIncompatibleConnectionsBoostUsageContextFactory = allowIncompatibleConnectionsBoostUsageContextFactory;
        }

        public BoostUsageContext Create(CommonUsageData param)
        {
            Type boostType = param.UsedBoost.GetType();

            BoostUsageContext createdBoostUsageContext = CreateByType(param, boostType);
            _createdBoostUsageContextNotifier.SendNotification(createdBoostUsageContext);

            return createdBoostUsageContext;
        }

        private BoostUsageContext CreateByType(CommonUsageData param, Type boostType)
        {
            if (boostType == typeof(SkipLevelBoost))
                return _skipLevelBoostUsageContextFactory.Create(param);

            if (boostType == typeof(SimplifyLevelBoost))
                return _simplifyLevelBoostUsageContextFactory.Create(param);

            if (boostType == typeof(AllowIncompatibleConnectionsBoost))
                return _allowIncompatibleConnectionsBoostUsageContextFactory.Create(param);

            return _boostUsageContextFactory.Create(param);
        }
    }
}
