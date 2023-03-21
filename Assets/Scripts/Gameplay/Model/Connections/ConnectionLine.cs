﻿using ConnectIt.Utilities.Collections;
using System;

namespace ConnectIt.Model
{
    public class ConnectionLine
    {
        public event Action<ConnectionLine> Removed;
        public event Action<ConnectionLine> UsingTilesChanged;

        public ClosedList<TileUser> UsingTiles { get; set; }
        public int CompatibilityIndex => _connection.First.CompatibilityIndex;

        private readonly Connection _connection;
        private readonly Action<TileUser> _addTileUserAction;

        public ConnectionLine(Port start)
        {
            _connection = new(start.Connectable);
            UsingTiles = new(_addTileUserAction);

            ExpandLine(start.UsingTile.Tile);
        }

        public void ExpandLine(Tile toTile)
        {
            TileUser tileUser = new(toTile, TileLayer.Line);

            _addTileUserAction(tileUser);

            UsingTilesChanged?.Invoke(this);
        }

        public void CompleteConnection(Port end)
        {
            _connection.CompleteConnection(end.Connectable);

            ExpandLine(end.UsingTile.Tile);
        }

        public void Remove()
        {
            _connection.RemoveConnection();
            
            foreach (TileUser tileUser in UsingTiles)
                tileUser.ResetTile();

            Removed?.Invoke(this);
        }
    }
}
