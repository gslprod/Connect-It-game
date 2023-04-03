using System;

namespace ConnectIt.Gameplay.Time
{
    public interface ITimeProvider
    {
        TimeSpan ElapsedTime { get; }
        float ElapsedTimeSec { get; }
        float DeltaTime { get; }
    }
}