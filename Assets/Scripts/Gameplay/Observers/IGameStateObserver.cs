using System;

namespace ConnectIt.Gameplay.Observers
{
    public interface IGameStateObserver
    {
        event Action GameCompleteProgressPercentsChanged;
        event Action MovesCountChanged;

        float GameCompleteProgressPercents { get; }
        int MovesCount { get; }
    }
}