using System;

namespace ConnectIt.Gameplay.Observers
{
    public interface IGameStateObserver
    {
        float GameCompleteProgressPercents { get; }

        event Action GameCompleteProgressPercentsChanged;
    }
}