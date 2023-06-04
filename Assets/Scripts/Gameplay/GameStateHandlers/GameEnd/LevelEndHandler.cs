using ConnectIt.Gameplay.Pause;
using ConnectIt.Utilities.Extensions.IPauseService;
using System;
using Zenject;

namespace ConnectIt.Gameplay.GameStateHandlers.GameEnd
{
    public enum LevelEndReason
    {
        None = 0,
        Win,
        Skip,
        ExitToMainMenu,
        Restart,
        GoToNextLevel
    }

    public class LevelEndHandler : ILevelEndHandler, IInitializable, IDisposable
    {
        public event Action<LevelEndReason> LevelEnded;

        public LevelEndReason EndReason { get; private set; } = LevelEndReason.None;
        public float ProgressPercentsToWin { get => _winHandler.ProgressPercentsToWin; set => _winHandler.ProgressPercentsToWin = value; }

        private readonly IRestartHandler.Factory _restartHandlerFactory;
        private readonly IWinHandler.Factory _winHandlerFactory;
        private readonly ISkipHandler.Factory _skipHandlerFactory;
        private readonly IExitToMainMenuHandler.Factory _exitToMainMenuHandlerFactory;
        private readonly IGoToNextLevelHandler.Factory _goToNextLevelHandlerFactory;
        private readonly IPauseService _pauseService;

        private IRestartHandler _restartHandler;
        private IWinHandler _winHandler;
        private ISkipHandler _skipHandler;
        private IExitToMainMenuHandler _exitToMainMenuHandler;
        private IGoToNextLevelHandler _goToNextLevelHandler;

        public LevelEndHandler(
            IRestartHandler.Factory restartHandlerFactory,
            IWinHandler.Factory winHandlerFactory,
            ISkipHandler.Factory skipHandlerFactory,
            IExitToMainMenuHandler.Factory exitToMainMenuHandlerFactory,
            IGoToNextLevelHandler.Factory goToNextLevelHandlerFactory,
            IPauseService pauseService)
        {
            _restartHandlerFactory = restartHandlerFactory;
            _winHandlerFactory = winHandlerFactory;
            _skipHandlerFactory = skipHandlerFactory;
            _exitToMainMenuHandlerFactory = exitToMainMenuHandlerFactory;
            _goToNextLevelHandlerFactory = goToNextLevelHandlerFactory;
            _pauseService = pauseService;
        }

        public void Initialize()
        {
            CreateHandlers();

            _restartHandler.Restarted += OnRestarted;
            _winHandler.Won += OnWon;
            _skipHandler.Skipped += OnSkipped;
            _exitToMainMenuHandler.ExitingToMainMenu += OnExitingToMainMenu;
            _goToNextLevelHandler.GoingToNextLevel += OnGoingToNextLevel;
        }

        public void Dispose()
        {
            _restartHandler.Restarted -= OnRestarted;
            _winHandler.Won -= OnWon;
            _skipHandler.Skipped -= OnSkipped;
            _exitToMainMenuHandler.ExitingToMainMenu -= OnExitingToMainMenu;
            _goToNextLevelHandler.GoingToNextLevel -= OnGoingToNextLevel;
        }

        public void RestartLevel()
        {
            _restartHandler.Restart();
        }

        public void WinLevel()
        {
            _winHandler.Win();
        }

        public void SkipLevel()
        {
            _skipHandler.Skip();
        }

        public void ExitToMainMenu()
        {
            _exitToMainMenuHandler.ExitToMainMenu();
        }

        public void GoToNextLevel()
        {
            _goToNextLevelHandler.GoToNextLevel();
        }

        private void EndLevel(LevelEndReason reason)
        {
            if (EndReason != LevelEndReason.None)
                return;

            EndReason = reason;
            _pauseService.SetPause(true, PauseEnablePriority.GameEndedState, this);

            LevelEnded?.Invoke(reason);
        }

        private void CreateHandlers()
        {
            _restartHandler = _restartHandlerFactory.Create();
            _winHandler = _winHandlerFactory.Create();
            _skipHandler = _skipHandlerFactory.Create();
            _exitToMainMenuHandler = _exitToMainMenuHandlerFactory.Create();
            _goToNextLevelHandler = _goToNextLevelHandlerFactory.Create();
        }

        private void OnRestarted()
        {
            EndLevel(LevelEndReason.Restart);
        }

        private void OnWon()
        {
            EndLevel(LevelEndReason.Win);
        }

        private void OnSkipped()
        {
            EndLevel(LevelEndReason.Skip);
        }

        private void OnExitingToMainMenu()
        {
            EndLevel(LevelEndReason.ExitToMainMenu);
        }

        private void OnGoingToNextLevel()
        {
            EndLevel(LevelEndReason.GoToNextLevel);
        }
    }
}
