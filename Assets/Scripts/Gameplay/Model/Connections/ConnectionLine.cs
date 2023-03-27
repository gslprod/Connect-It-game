using ConnectIt.Infrastructure.CreatedObjectNotifiers;
using ConnectIt.Infrastructure.Dispose;
using ConnectIt.Utilities.Extensions;
using System;
using System.Collections.Generic;
using Zenject;

namespace ConnectIt.Gameplay.Model
{
    public class ConnectionLine : IDisposeNotifier<ConnectionLine>, IInitializable
    {
        public event Action<ConnectionLine> Disposing;
        public event Action<ConnectionLine> UsingTilesChanged;

        public IEnumerable<TileUser> UsingTiles => _usingTiles;
        public int CompatibilityIndex => _connection.First.CompatibilityIndex;
        public bool ConnectionCompleted => _connection.ConnectionCompleted;

        private readonly List<TileUser> _usingTiles = new();
        private readonly Port _start;
        private readonly ICreatedObjectNotifier<ConnectionLine> _createdConnectionLineNotifier;

        private Connection _connection;

        public ConnectionLine(Port start,
            ICreatedObjectNotifier<ConnectionLine> createdConnectionLineNotifier)
        {
            _start = start;
            _createdConnectionLineNotifier = createdConnectionLineNotifier;
        }

        public void Initialize()
        {
            _connection = new(_start.Connectable);

            ExpandLine(_start.UsingTile.Tile);

            _createdConnectionLineNotifier.SendNotification(this);
        }

        public void ExpandLine(Tile toTile)
        {
            TileUser tileUser = new(toTile, TileLayer.Line);

            _usingTiles.Add(tileUser);
            tileUser.TileUserInfoRequest += OnTileUserInfoRequest;

            UsingTilesChanged?.Invoke(this);
        }

        public bool CanBeCompletedWith(Port port)
        {
            return _connection.First.CanBeConnectedWith(port.Connectable);
        }

        public void CompleteConnection(Port end)
        {
            _connection.CompleteConnection(end.Connectable);

            ExpandLine(end.UsingTile.Tile);
        }

        public void Dispose()
        {
            _connection.Dispose();

            foreach (TileUser tileUser in UsingTiles)
            {
                tileUser.Dispose();
                tileUser.TileUserInfoRequest -= OnTileUserInfoRequest;
            }

            Disposing?.Invoke(this);
        }

        private object OnTileUserInfoRequest(TileUser usingTile)
            => new ConnectionLineTileUserObjectInfo(this, UsingTiles.IndexOf(usingTile));

        public class ConnectionLineTileUserObjectInfo
        {
            public ConnectionLine ConnectionLine { get; }
            public int UsingTileIndex { get; }

            public ConnectionLineTileUserObjectInfo(ConnectionLine connectionLine, int usingTileIndex)
            {
                ConnectionLine = connectionLine;
                UsingTileIndex = usingTileIndex;
            }
        }

        public class Factory : PlaceholderFactory<Port, ConnectionLine> { }
    }
}
