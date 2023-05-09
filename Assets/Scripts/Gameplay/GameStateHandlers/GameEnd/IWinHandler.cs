using System;
using Zenject;

namespace ConnectIt.Gameplay.GameStateHandlers.GameEnd
{
    public interface IWinHandler
    {
        event Action Won;

        void Win();

        public class Factory : PlaceholderFactory<IWinHandler> { }
    }
}