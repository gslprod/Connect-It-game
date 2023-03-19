using ConnectIt.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ConnectIt.Model
{
    public class Tilemaps
    {
        private readonly TilemapLayerSet[] _maps;
        private readonly List<Tile> _tiles = new();

        public Tilemaps(TilemapLayerSet[] maps)
        {
            Validate(maps);

            _maps = maps;
        }

        public Tile GetTileAtPosition(Vector3 position)
        {
            return null;
        }

        private void Validate(TilemapLayerSet[] maps)
        {
            IEnumerable<IGrouping<TileLayer, TilemapLayerSet>> groupsByLayer = _maps.GroupBy(set => set.Layer);

            int groupsWithDuplicateLayersCount =
                groupsByLayer.Where(group => group.Count() > 1)
                .Count();

            bool containsMapLayer = groupsByLayer.Any(group => group.Key == TileLayer.Map);

            Assert.That(groupsWithDuplicateLayersCount == 0 &&
                containsMapLayer);
        }

        private void CreateTiles()
        {
            Tilemap mapTilemap = FindSetByLayer(TileLayer.Map).Tilemap;

            mapTilemap.CompressBounds();
            BoundsInt mapCellBounds = mapTilemap.cellBounds;
            Vector3Int boundsSize = mapCellBounds.size;

            TileBase[] tiles = new TileBase[boundsSize.x * boundsSize.y * boundsSize.z];
            mapTilemap.GetTilesBlockNonAlloc(mapCellBounds, tiles);
        }

        private TilemapLayerSet FindSetByLayer(TileLayer layer)
            => _maps.First(set => set.Layer == layer);
    }

    public class TilemapLayerSet
    {
        public Tilemap Tilemap { get; set; }
        public TileLayer Layer { get; set; }
    }
}
