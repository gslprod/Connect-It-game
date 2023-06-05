using ConnectIt.Gameplay.Observers.Internal;
using ConnectIt.Shop.Goods.Boosts.UsageContext;
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
        public event Action<BoostUsageContext> BoostUsed
        {
            add => _usedBoostsObserver.BoostUsed += value;
            remove => _usedBoostsObserver.BoostUsed -= value;
        }

        public float GameCompleteProgressPercents => _gameProgressObserver.GameCompleteProgressPercents;
        public int MovesCount => _movesObserver.MovesCount;
        public int BoostsUsageCount => _usedBoostsObserver.BoostsUsageCount;
        public bool AnyBoostWasUsed => _usedBoostsObserver.AnyBoostWasUsed;

        private readonly GameProgressObserver _gameProgressObserver;
        private readonly MovesObserver _movesObserver;
        private readonly UsedBoostsObserver _usedBoostsObserver;

        public GameStateObserver(
            GameProgressObserver gameProgressObserver,
            MovesObserver movesObserver,
            UsedBoostsObserver usedBoostsObserver)
        {
            _gameProgressObserver = gameProgressObserver;
            _movesObserver = movesObserver;
            _usedBoostsObserver = usedBoostsObserver;
        }
    }
}
