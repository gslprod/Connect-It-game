namespace ConnectIt.Model
{
    public class Port
    {
        public Connectable Connectable { get; }
        public TileUser UsingTile { get; }

        public Port(Tile position, int compatibilityIndex)
        {
            UsingTile = new TileUser(position, TileLayer.Main);
            Connectable = new Connectable(compatibilityIndex);

            UsingTile.TileUserInfoRequest += OnTileUserInfoRequest;
        }

        public void OnDestroy()
        {
            UsingTile.TileUserInfoRequest -= OnTileUserInfoRequest;
        }

        private object OnTileUserInfoRequest(TileUser usingTile)
            => new PortTileUserObjectInfo(this);

        public class PortTileUserObjectInfo
        {
            public Port Port { get; }

            public PortTileUserObjectInfo(Port port)
            {
                Port = port;
            }
        }
    }
}
