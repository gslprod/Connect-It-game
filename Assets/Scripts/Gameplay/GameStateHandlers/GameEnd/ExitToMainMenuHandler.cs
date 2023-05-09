using ConnectIt.Scenes;
using ConnectIt.Scenes.Switchers;
using System;
using Zenject;

namespace ConnectIt.Gameplay.GameStateHandlers.GameEnd
{
    public class ExitToMainMenuHandler : IExitToMainMenuHandler
    {
        public event Action ExitingToMainMenu;

        private readonly ISceneSwitcher _sceneSwitcher;

        public ExitToMainMenuHandler(
            ISceneSwitcher sceneSwitcher)
        {
            _sceneSwitcher = sceneSwitcher;
        }

        public void Dispose()
        {

        }

        public void ExitToMainMenu()
        {
            ExitingToMainMenu?.Invoke();

            _sceneSwitcher.TryGoToScene(SceneType.MenuScene);
        }
    }
}
