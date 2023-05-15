using ConnectIt.Gameplay.Observers;
using ConnectIt.Stats.Data;
using System;
using Zenject;

namespace ConnectIt.Stats.Modules
{
    public class MovesCountStatsModule : IStatsModule, IInitializable, IDisposable
    {
        private readonly IStatsCenter _statsCenter;
        private readonly IGameStateObserver _gameStateObserver;

        private MovesCountStatsData _data;
        private int _lastMovesCount = 0;

        public MovesCountStatsModule(
            IStatsCenter statsCenter,
            IGameStateObserver gameStateObserver)
        {
            _statsCenter = statsCenter;
            _gameStateObserver = gameStateObserver;
        }

        public void Initialize()
        {
            _data = _statsCenter.GetData<MovesCountStatsData>();
            _statsCenter.RegisterModule(this);

            _gameStateObserver.MovesCountChanged += OnMovesCountChanged;
        }

        public void Dispose()
        {
            _statsCenter.UnregisterModule(this);

            _gameStateObserver.MovesCountChanged -= OnMovesCountChanged;
        }

        private void OnMovesCountChanged()
        {
            _data.InscreaseRawValue(_gameStateObserver.MovesCount - _lastMovesCount);

            _lastMovesCount = _gameStateObserver.MovesCount;
        }
    }
}
