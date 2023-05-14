using ConnectIt.Localization;
using System;

namespace ConnectIt.Stats.Data
{
    public interface IStatsData
    {
        event Action<IStatsData> ValueChanged;

        TextKey Name { get; }
        TextKey Description { get; }
        TextKey Value { get; }
        bool OftenUpdating { get; }
    }
}
