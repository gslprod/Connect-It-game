using System.Collections.Generic;

namespace ConnectIt.Model
{
    public class ConnectionLine
    {
        private List<TileUser> _usingTiles = new();
        private Connection _connection;

        public ConnectionLine(Port start)
        {
            _connection = new(start.Connectable);

            ExpandLine(start.UsingTile.Tile);
        }

        public void ExpandLine(Tile toTile)
        {
            TileUser tileUser = new(toTile, TileLayers.Line);

            _usingTiles.Add(tileUser);
        }
    }
}
