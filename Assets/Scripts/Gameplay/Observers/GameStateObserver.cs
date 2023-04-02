using ConnectIt.Config;
using ConnectIt.Gameplay.Model;
using ConnectIt.Infrastructure.Registrators;
using System;
using Zenject;

namespace ConnectIt.Gameplay.Observers
{
    public class GameStateObserver : IInitializable, IGameStateObserver
    {
        public event Action GameCompleteProgressPercentsChanged;

        public float GameCompleteProgressPercents =>
            (float)_connectedPortsCount / _portsCount * _connectedPortsWeightPercents +
            (float)_usedByLineTilesCount / _tilesCount * _usedTilesWeightPercents;

        private float _connectedPortsWeightPercents => (1f - _gameplayLogicConfig.UsedTilesVsConnectedPortsGameCompleteFactor) * 100;
        private float _usedTilesWeightPercents => _gameplayLogicConfig.UsedTilesVsConnectedPortsGameCompleteFactor * 100;

        private readonly Tilemaps _tilemaps;
        private readonly IRegistrator<Port> _portRegistrator;
        private readonly GameplayLogicConfig _gameplayLogicConfig;

        private int _portsCount;
        private int _connectedPortsCount;
        private int _tilesCount;
        private int _usedByLineTilesCount;

        public GameStateObserver(Tilemaps tilemaps,
            IRegistrator<Port> portRegistrator,
            GameplayLogicConfig gameplayLogicConfig)
        {
            _tilemaps = tilemaps;
            _portRegistrator = portRegistrator;
            _gameplayLogicConfig = gameplayLogicConfig;
        }

        public void Initialize()
        {
            foreach (var tile in _tilemaps.Tiles)
            {
                _tilesCount++;
                if (tile.UserInLayerExists(TileLayer.Line))
                    _usedByLineTilesCount++;

                tile.UsersChanged += OnTilemapUsersChanged;
                tile.Disposing += OnTileDisposing;
            }

            foreach (var port in _portRegistrator.Registrations)
            {
                _portsCount++;
                if (port.Connectable.HasConnection)
                    _connectedPortsCount++;

                port.Connectable.ConnectionChanged += OnPortConnectionChanged;
                port.Disposing += OnPortDisposing;
            }

            GameCompleteProgressPercentsChanged?.Invoke();
        }

        private void OnTilemapUsersChanged(Tile tile, TileLayer tileLayer)
        {
            if (tileLayer != TileLayer.Line)
                return;

            _usedByLineTilesCount += tile.UserInLayerExists(TileLayer.Line) ? 1 : -1;

            GameCompleteProgressPercentsChanged?.Invoke();
        }

        private void OnPortConnectionChanged(Connectable connectable)
        {
            _connectedPortsCount += connectable.HasConnection ? 1 : -1;

            GameCompleteProgressPercentsChanged?.Invoke();
        }

        private void OnTileDisposing(Tile tile)
        {
            tile.UsersChanged -= OnTilemapUsersChanged;
            tile.Disposing -= OnTileDisposing;

            _tilesCount--;
            if (tile.UserInLayerExists(TileLayer.Line))
                _usedByLineTilesCount--;

            GameCompleteProgressPercentsChanged?.Invoke();
        }

        private void OnPortDisposing(Port port)
        {
            port.Connectable.ConnectionChanged -= OnPortConnectionChanged;
            port.Disposing -= OnPortDisposing;

            _portsCount--;
            if (port.Connectable.HasConnection)
                _connectedPortsCount--;

            GameCompleteProgressPercentsChanged?.Invoke();
        }
    }
}
