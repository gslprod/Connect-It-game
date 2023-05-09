using System;
using Zenject;

namespace ConnectIt.Gameplay.GameStateHandlers.GameEnd
{
    public interface IExitToMainMenuHandler : IDisposable
    {
        event Action ExitingToMainMenu;

        void ExitToMainMenu();

        public class Factory : PlaceholderFactory<IExitToMainMenuHandler> { }
    }
}