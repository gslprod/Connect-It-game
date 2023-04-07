using ConnectIt.Infrastructure.Setters;
using System;
using Zenject;

namespace ConnectIt.Gameplay.Pause
{
    public class PauseService : IPauseService, IInitializable
    {
        public event Action<bool> PauseChanged;

        public bool Paused { get; private set; }

        private PriorityAwareSetter<bool> _isPausedSetter;

        public void Initialize()
        {
            _isPausedSetter = new(
                SetPauseInternal,
                () => Paused,
                false);
        }

        public void SetPause(bool isPause, int priority)
        {
            _isPausedSetter.SetValue(isPause, priority);
        }

        public void ResetPauseWithPriority(int priority)
        {
            _isPausedSetter.ResetValueWithPriority(priority);
        }

        private void SetPauseInternal(bool newValue)
        {
            Paused = newValue;
            PauseChanged?.Invoke(Paused);
        }
    }
}
