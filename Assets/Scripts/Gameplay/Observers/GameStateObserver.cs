using ConnectIt.Gameplay.Observers.Internal;
using System;

namespace ConnectIt.Gameplay.Observers
{
    public class GameStateObserver : IGameStateObserver
    {
        public event Action GameCompleteProgressPercentsChanged
        {
            add => _gameProgressObserver.GameCompleteProgressPercentsChanged += value;
            remove => _gameProgressObserver.GameCompleteProgressPercentsChanged -= value;
        }
        public event Action MovesCountChanged
        {
            add => _movesObserver.MovesCountChanged += value;
            remove => _movesObserver.MovesCountChanged -= value;
        }

        public float GameCompleteProgressPercents => _gameProgressObserver.GameCompleteProgressPercents;
        public int MovesCount => _movesObserver.MovesCount;

        private readonly GameProgressObserver _gameProgressObserver;
        private readonly MovesObserver _movesObserver;

        public GameStateObserver(
            GameProgressObserver gameProgressObserver,
            MovesObserver movesObserver)
        {
            _gameProgressObserver = gameProgressObserver;
            _movesObserver = movesObserver;
        }
    }
}
