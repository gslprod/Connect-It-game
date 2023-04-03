using System;
using Zenject;
using UnityTime = UnityEngine.Time;

namespace ConnectIt.Gameplay.Time
{
    public class TimeProvider : ITimeProvider, ITickable
    {
        public TimeSpan ElapsedTime => TimeSpan.FromSeconds(ElapsedTimeSec);
        public float ElapsedTimeSec { get; private set; } = 0f;

        public float DeltaTime => UnityTime.deltaTime;

        public void Tick()
        {
            ElapsedTimeSec += DeltaTime;
        }
    }
}
