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

        public void SetPause(bool isPause, int priority, object source)
        {
            _isPausedSetter.SetValue(isPause, priority, source);
        }

        public void ResetPause(object source)
        {
            _isPausedSetter.ResetValue(source);
        }

        private void SetPauseInternal(bool newValue)
        {
            Paused = newValue;
            PauseChanged?.Invoke(Paused);
        }
    }
}
