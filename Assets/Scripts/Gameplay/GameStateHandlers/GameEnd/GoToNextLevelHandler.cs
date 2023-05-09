using ConnectIt.Config;
using ConnectIt.Scenes.Switchers;
using System;

namespace ConnectIt.Gameplay.GameStateHandlers.GameEnd
{
    public class GoToNextLevelHandler : IGoToNextLevelHandler
    {
        public event Action GoingToNextLevel;

        private readonly ISceneSwitcher _sceneSwitcher;
        private readonly GameplayLogicConfig _gameplayLogicConfig;

        public GoToNextLevelHandler(
            ISceneSwitcher sceneSwitcher,
            GameplayLogicConfig gameplayLogicConfig)
        {
            _sceneSwitcher = sceneSwitcher;
            _gameplayLogicConfig = gameplayLogicConfig;
        }

        public void Dispose()
        {

        }

        public void GoToNextLevel()
        {
            _gameplayLogicConfig.SetCurrentLevel(_gameplayLogicConfig.CurrentLevel + 1);

            GoingToNextLevel?.Invoke();

            _sceneSwitcher.TryReloadActiveScene();
        }
    }
}
