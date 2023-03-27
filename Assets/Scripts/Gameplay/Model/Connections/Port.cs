using ConnectIt.Infrastructure.CreatedObjectNotifiers;
using ConnectIt.Infrastructure.Dispose;
using System;
using UnityEngine.UIElements;
using Zenject;

namespace ConnectIt.Model
{
    public class Port : IDisposeNotifier<Port>, IInitializable
    {
        public event Action<Port> Disposing;

        public Connectable Connectable { get; private set; }
        public TileUser UsingTile { get; private set; }

        private readonly Tile _position;
        private int _compatibilityIndex;
        private readonly ICreatedObjectNotifier<Port> _createdPortNotifier;

        public Port(Tile position,
            int compatibilityIndex,
            ICreatedObjectNotifier<Port> createdPortNotifier)
        {
            _position = position;
            _compatibilityIndex = compatibilityIndex;

            _createdPortNotifier = createdPortNotifier;
        }

        public void Initialize()
        {
            UsingTile = new TileUser(_position, TileLayer.Main);
            Connectable = new Connectable(_compatibilityIndex);

            UsingTile.TileUserInfoRequest += OnTileUserInfoRequest;

            _createdPortNotifier.SendNotification(this);
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

        public class Factory : PlaceholderFactory<Tile, int, Port> { }
    }
}
