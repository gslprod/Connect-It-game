using System.Collections.Generic;

namespace ConnectIt.Model
{
    public class ConnectionLine
    {
        private readonly List<TileUser> _usingTiles = new();
        private Connection _connection;

        public ConnectionLine(Port start)
        {
            _connection = new(start.Connectable);

            ExpandLine(start.UsingTile.Tile);
        }

        public void ExpandLine(Tile toTile)
        {
            TileUser tileUser = new(toTile, TileLayer.Line);

            _usingTiles.Add(tileUser);
        }
    }
}
