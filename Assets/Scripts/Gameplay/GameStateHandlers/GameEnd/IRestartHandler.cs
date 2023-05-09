using System;
using Zenject;

namespace ConnectIt.Gameplay.GameStateHandlers.GameEnd
{
    public interface IRestartHandler : IDisposable
    {
        event Action Restarted;

        void Restart();

        public class Factory : PlaceholderFactory<IRestartHandler> { }
    }
}