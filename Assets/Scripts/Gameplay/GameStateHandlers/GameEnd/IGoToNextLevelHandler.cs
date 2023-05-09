using System;
using Zenject;

namespace ConnectIt.Gameplay.GameStateHandlers.GameEnd
{
    public interface IGoToNextLevelHandler : IDisposable
    {
        event Action GoingToNextLevel;

        void GoToNextLevel();

        public class Factory : PlaceholderFactory<IGoToNextLevelHandler> { }
    }
}