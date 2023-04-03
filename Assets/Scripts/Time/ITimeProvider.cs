namespace ConnectIt.Time
{
    public interface ITimeProvider
    {
        float DeltaTimeSec { get; }
        float SinceStartupSec { get; }
    }
}