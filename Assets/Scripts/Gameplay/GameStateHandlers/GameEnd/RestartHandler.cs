using ConnectIt.Scenes.Switchers;
using System;

namespace ConnectIt.Gameplay.GameStateHandlers.GameEnd
{
    public class RestartHandler : IRestartHandler
    {
        public event Action Restarted;

        private readonly ISceneSwitcher _sceneSwitcher;

        public RestartHandler(
            ISceneSwitcher sceneSwitcher)
        {
            _sceneSwitcher = sceneSwitcher;
        }

        public void Dispose()
        {

        }

        public void Restart()
        {
            Restarted?.Invoke();

            _sceneSwitcher.TryReloadActiveScene();
        }
    }
}
