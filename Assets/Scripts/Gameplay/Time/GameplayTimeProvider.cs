using ConnectIt.Time;
using System;
using Zenject;

namespace ConnectIt.Gameplay.Time
{
    public class GameplayTimeProvider : IGameplayTimeProvider, ITickable
    {
        public TimeSpan ElapsedTime => TimeSpan.FromSeconds(ElapsedTimeSec);
        public float ElapsedTimeSec { get; private set; } = 0f;

        public float DeltaTimeSec => _globalTimeProvider.DeltaTimeSec;
        public float SinceStartupSec => _globalTimeProvider.SinceStartupSec;

        private readonly ITimeProvider _globalTimeProvider;

        public GameplayTimeProvider(ITimeProvider globalTimeProvider)
        {
            _globalTimeProvider = globalTimeProvider;
        }

        public void Tick()
        {
            ElapsedTimeSec += DeltaTimeSec;
        }
    }
}
