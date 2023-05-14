using ConnectIt.Stats.Data;
using ConnectIt.Time;
using System;
using Zenject;

namespace ConnectIt.Stats.Modules
{
    public class ApplicationRunningTimeStatsModule : IStatsModule, IInitializable, ITickable, IDisposable
    {
        private readonly IStatsCenter _statsCenter;
        private readonly ITimeProvider _timeProvider;

        private ApplicationRunningTimeStatsData _data;

        public ApplicationRunningTimeStatsModule(
            IStatsCenter statsCenter,
            ITimeProvider timeProvider)
        {
            _statsCenter = statsCenter;
            _timeProvider = timeProvider;
        }

        public void Initialize()
        {
            _data = _statsCenter.GetData<ApplicationRunningTimeStatsData>();
            _statsCenter.RegisterModule(this);
        }

        public void Tick()
        {
            _data.InscreaseRawValue(_timeProvider.DeltaTimeSec);
        }

        public void Dispose()
        {
            _statsCenter.UnregisterModule(this);
        }
    }
}
