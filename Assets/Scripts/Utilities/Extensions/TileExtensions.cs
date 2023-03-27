using ConnectIt.Gameplay.Model;

namespace ConnectIt.Utilities.Extensions
{
    public static class TileExtensions
    {
        public static bool IsNearTo(this Tile source, Tile target, int nearDistanceIncluding)
            => source.LocationInTileMap.IsNearTo(target.LocationInTileMap, nearDistanceIncluding);
    }
}
