using System;

namespace ConnectIt.Gameplay.GameStateHandlers.GameEnd
{
    public interface ILevelEndHandler
    {
        LevelEndReason EndReason { get; }
        float ProgressPercentsToWin { get; set; }

        event Action<LevelEndReason> LevelEnded;

        void ExitToMainMenu();
        void GoToNextLevel();
        void RestartLevel();
        void SkipLevel();
        void WinLevel();
    }
}