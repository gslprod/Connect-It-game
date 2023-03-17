namespace ConnectIt.Model
{
    public abstract class TileUser
    {
        public Tile Tile { get; }

        public abstract bool CanBeRemoved { get; }

        public TileUser(Tile tile)
        {
            Tile = tile;
        }

        public virtual bool CanBeOtherUserAdded(TileUser other)
            => false;
    }
}
