using System;

namespace ConnectIt.Gameplay.Pause
{
    public interface IPauseService
    {
        bool Paused { get; }

        event Action<bool> PauseChanged;

        void SetPause(bool isPause, int priority);
        void ResetPauseWithPriority(int priority);
    }
}