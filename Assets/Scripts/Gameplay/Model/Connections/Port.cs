using ConnectIt.Infrastructure.Dispose;
using System;

namespace ConnectIt.Model
{
    public class Port : IDisposeNotifier<Port>
    {
        public event Action<Port> Disposing;

        public Connectable Connectable { get; }
        public TileUser UsingTile { get; }

        public Port(Tile position, int compatibilityIndex)
        {
            UsingTile = new TileUser(position, TileLayer.Main);
            Connectable = new Connectable(compatibilityIndex);

            UsingTile.TileUserInfoRequest += OnTileUserInfoRequest;
        }

        public void Dispose()
        {
            UsingTile.TileUserInfoRequest -= OnTileUserInfoRequest;

            Connectable.Dispose();
            UsingTile.Dispose();

            Disposing?.Invoke(this);
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
