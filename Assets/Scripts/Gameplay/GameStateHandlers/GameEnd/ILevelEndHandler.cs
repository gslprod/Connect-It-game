using System;

namespace ConnectIt.Gameplay.GameStateHandlers.GameEnd
{
    public interface ILevelEndHandler
    {
        LevelEndReason EndReason { get; }

        event Action<LevelEndReason> LevelEnded;

        void ExitToMainMenu();
        void GoToNextLevel();
        void RestartLevel();
        void SkipLevel();
        void WinLevel();
    }
}