using ConnectIt.Gameplay.Pause;
using ConnectIt.Time;
using System;
using Zenject;

namespace ConnectIt.Gameplay.Time
{
    public class GameplayTimeProvider : IGameplayTimeProvider, IInitializable, ITickable, IDisposable
    {
        public TimeSpan ElapsedTime => TimeSpan.FromSeconds(ElapsedTimeSec);
        public float ElapsedTimeSec { get; private set; } = 0f;

        public float DeltaTimeSec => _globalTimeProvider.DeltaTimeSec;
        public float SinceStartupSec => _globalTimeProvider.SinceStartupSec;

        private readonly ITimeProvider _globalTimeProvider;
        private readonly IPauseService _pauseService;

        private bool _paused;

        public GameplayTimeProvider(ITimeProvider globalTimeProvider, IPauseService pauseService)
        {
            _globalTimeProvider = globalTimeProvider;
            _pauseService = pauseService;
        }

        public void Initialize()
        {
            _pauseService.PauseChanged += OnPauseChanged;
        }

        public void Tick()
        {
            if (_paused)
                return;

            ElapsedTimeSec += DeltaTimeSec;
        }

        public void Dispose()
        {
            _pauseService.PauseChanged -= OnPauseChanged;
        }

        private void OnPauseChanged(bool paused)
        {
            _paused = paused;
        }
    }
}
