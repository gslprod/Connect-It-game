using UnityTime = UnityEngine.Time;

namespace ConnectIt.Time
{
    public class TimeProvider : ITimeProvider
    {
        public float SinceStartupSec => UnityTime.realtimeSinceStartup;
        public float DeltaTimeSec => UnityTime.deltaTime;
    }
}
