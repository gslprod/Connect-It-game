using System;

namespace ConnectIt.Gameplay.Pause
{
    public class PauseService : IPauseService
    {
        public event Action<bool> PauseChanged;

        public bool Paused { get; private set; } = false;

        public void SetPause(bool isPause)
        {
            if (isPause == Paused)
                return;

            Paused = isPause;
            PauseChanged?.Invoke(Paused);
        }
    }
}
