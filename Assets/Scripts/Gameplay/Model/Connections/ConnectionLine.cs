using ConnectIt.Infrastructure.Dispose;
using ConnectIt.Utilities.Extensions;
using System;
using System.Collections.Generic;

namespace ConnectIt.Model
{
    public class ConnectionLine : IDisposeNotifier<ConnectionLine>
    {
        public event Action<ConnectionLine> Disposing;
        public event Action<ConnectionLine> UsingTilesChanged;

        public IEnumerable<TileUser> UsingTiles => _usingTiles;
        public int CompatibilityIndex => _connection.First.CompatibilityIndex;
        public bool ConnectionCompleted => _connection.ConnectionCompleted;

        private readonly List<TileUser> _usingTiles = new();
        private readonly Connection _connection;

        public ConnectionLine(Port start)
        {
            _connection = new(start.Connectable);

            ExpandLine(start.UsingTile.Tile);
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
    }
}
