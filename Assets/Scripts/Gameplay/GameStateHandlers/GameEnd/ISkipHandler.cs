using System;
using Zenject;

namespace ConnectIt.Gameplay.GameStateHandlers.GameEnd
{
    public interface ISkipHandler : IDisposable
    {
        event Action Skipped;

        void Skip();

        public class Factory : PlaceholderFactory<ISkipHandler> { }
    }
}