using System;

namespace ConnectIt.Gameplay.Observers
{
    public interface IGameStateObserver
    {
        event Action GameCompleteProgressPercentsChanged;

        float GameCompleteProgressPercents { get; }
    }
}