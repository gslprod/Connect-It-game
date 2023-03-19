﻿namespace ConnectIt.Model
{
    public class Port
    {
        public Connectable Connectable { get; }
        public TileUser UsingTile { get; }

        public Port(Tile position, int compatibilityIndex)
        {
            UsingTile = new TileUser(position, TileLayer.Main);
            Connectable = new Connectable(compatibilityIndex);
        }
    }
}
