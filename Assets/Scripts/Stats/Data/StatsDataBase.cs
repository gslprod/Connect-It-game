using ConnectIt.Localization;
using System;

namespace ConnectIt.Stats.Data
{
    public abstract class StatsDataBase<T> : IStatsData
    {
        public abstract event Action<IStatsData> ValueChanged;
        public abstract event Action<StatsDataBase<T>> RawValueChanged;

        public abstract TextKey Name { get; }
        public abstract TextKey Description { get; }
        public abstract TextKey Value { get; }
        public abstract T RawValue { get; set; }
        public abstract bool OftenUpdating { get; }
    }
}
