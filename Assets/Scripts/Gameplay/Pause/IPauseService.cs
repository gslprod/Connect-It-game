using System;

namespace ConnectIt.Gameplay.Pause
{
    public interface IPauseService
    {
        bool Paused { get; }

        event Action<bool> PauseChanged;

        void SetPause(bool isPause, int priority, object source);
        void ResetPause(object source);
    }
}