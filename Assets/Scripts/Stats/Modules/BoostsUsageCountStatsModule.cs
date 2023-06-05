using ConnectIt.Gameplay.Observers;
using ConnectIt.Shop.Goods.Boosts.UsageContext;
using ConnectIt.Stats.Data;
using System;
using Zenject;

namespace ConnectIt.Stats.Modules
{
    public class BoostsUsageCountStatsModule : IStatsModule, IInitializable, IDisposable
    {
        private readonly IStatsCenter _statsCenter;
        private readonly IGameStateObserver _gameStateObserver;

        private BoostsUsageCountStatsData _data;
        private int _lastBoostsUsageCount = 0;

        public BoostsUsageCountStatsModule(
            IStatsCenter statsCenter,
            IGameStateObserver gameStateObserver)
        {
            _statsCenter = statsCenter;
            _gameStateObserver = gameStateObserver;
        }

        public void Initialize()
        {
            _data = _statsCenter.GetData<BoostsUsageCountStatsData>();
            _statsCenter.RegisterModule(this);

            _gameStateObserver.BoostUsed += OnBoostUsed;
        }

        public void Dispose()
        {
            _statsCenter.UnregisterModule(this);

            _gameStateObserver.BoostUsed -= OnBoostUsed;
        }

        private void OnBoostUsed(BoostUsageContext usageContext)
        {
            _data.InscreaseRawValue(_gameStateObserver.BoostsUsageCount - _lastBoostsUsageCount);

            _lastBoostsUsageCount = _gameStateObserver.BoostsUsageCount;
        }
    }
}
