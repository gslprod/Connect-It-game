using ConnectIt.Stats.Data;
using ConnectIt.Stats.Modules;
using System;
using System.Collections.Generic;

namespace ConnectIt.Stats
{
    public interface IStatsCenter
    {
        IEnumerable<IStatsData> StatsData { get; }

        IStatsData GetData(Type type);
        T GetData<T>() where T : IStatsData;
        void RegisterModule(IStatsModule module);
        void UnregisterModule(IStatsModule module);
    }
}