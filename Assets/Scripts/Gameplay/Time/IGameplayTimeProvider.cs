using ConnectIt.Time;
using System;

namespace ConnectIt.Gameplay.Time
{
    public interface IGameplayTimeProvider : ITimeProvider
    {
        TimeSpan ElapsedTime { get; }
        float ElapsedTimeSec { get; }
    }
}