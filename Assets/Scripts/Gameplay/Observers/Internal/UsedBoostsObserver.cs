using ConnectIt.Infrastructure.CreatedObjectNotifiers;
using ConnectIt.Shop.Goods.Boosts.UsageContext;
using System;
using Zenject;

namespace ConnectIt.Gameplay.Observers.Internal
{
    public class UsedBoostsObserver : IInitializable, IDisposable
    {
        public event Action<BoostUsageContext> BoostUsed;

        public int BoostsUsageCount { get; private set; } = 0;
        public bool AnyBoostWasUsed => BoostsUsageCount > 0;

        private readonly ICreatedObjectNotifier<BoostUsageContext> _createdBoostUsageContextNotifier;

        public UsedBoostsObserver(
            ICreatedObjectNotifier<BoostUsageContext> createdBoostUsageContextNotifier)
        {
            _createdBoostUsageContextNotifier = createdBoostUsageContextNotifier;
        }

        public void Initialize()
        {
            _createdBoostUsageContextNotifier.Created += OnBoostUsageContextCreated;
        }

        public void Dispose()
        {
            _createdBoostUsageContextNotifier.Created -= OnBoostUsageContextCreated;
        }

        private void OnBoostUsageContextCreated(BoostUsageContext context)
        {
            BoostsUsageCount++;
            BoostUsed?.Invoke(context);
        }
    }
}
