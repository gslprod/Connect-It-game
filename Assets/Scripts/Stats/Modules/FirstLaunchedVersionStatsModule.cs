using ConnectIt.Config;
using ConnectIt.Stats.Data;
using System;
using Zenject;

namespace ConnectIt.Stats.Modules
{
    public class FirstLaunchedVersionStatsModule : IStatsModule, IInitializable, IDisposable
    {
        private readonly IStatsCenter _statsCenter;
        private readonly GameVersion _version;

        private FirstLaunchedVersionStatsData _data;

        public FirstLaunchedVersionStatsModule(
            IStatsCenter statsCenter,
            GameVersion version)
        {
            _statsCenter = statsCenter;
            _version = version;
        }

        public void Initialize()
        {
            _data = _statsCenter.GetData<FirstLaunchedVersionStatsData>();
            _statsCenter.RegisterModule(this);

            if (string.IsNullOrEmpty(_data.RawValue))
                _data.RawValue = _version.GetVersion();
        }

        public void Dispose()
        {
            _statsCenter.UnregisterModule(this);
        }
    }
}
