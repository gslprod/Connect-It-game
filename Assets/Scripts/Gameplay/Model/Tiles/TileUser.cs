using ConnectIt.Utilities;

namespace ConnectIt.Model
{
    public class TileUser
    {
        public bool HasTile => Tile != null;

        public Tile Tile { get; private set; }
        public TileLayers Layer { get; private set; }

        public TileUser(TileLayers layer)
        {
            Layer = layer;
        }

        public TileUser(Tile tile, TileLayers layer)
        {
            Layer = layer;

            SetTile(tile);
        }

        public void SetTile(Tile tile)
        {
            Assert.IsNotNull(tile);
            Assert.That(!HasTile);

            Tile.AddUser(this);
            Tile = tile;
        }

        public void ChangeTile(Tile tile)
        {
            if (HasTile)
                ResetTile();

            SetTile(tile);
        }

        public void ResetTile()
        {
            Assert.That(HasTile);

            Tile.RemoveUser(this);
            Tile = null;
        }

        public void SetLayer(TileLayers newLayer)
        {
            if (Layer == newLayer)
                return;

            Assert.That(CanLayerBeChangedTo(newLayer));

            Layer = newLayer;
        }

        public bool CanLayerBeChangedTo(TileLayers newLayer)
            => !IsLayerIsBusy(newLayer);

        private bool IsLayerIsBusy(TileLayers layer)
            => HasTile && Tile.UserInLayerExists(layer);
    }
}
