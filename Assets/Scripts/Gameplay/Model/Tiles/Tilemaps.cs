using ConnectIt.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ConnectIt.Model
{
    public class Tilemaps
    {
        public event Action<Tile, TileLayer> OnTileBaseChanged;

        private readonly TilemapLayerSet[] _maps;
        private readonly List<Tile> _tiles = new();

        public Tilemaps(TilemapLayerSet[] maps)
        {
            Validate(maps);
            _maps = maps;

            CreateTiles();
        }

        public Tile GetTileAtWorldPosition(Vector3 worldPosition)
        {
            Tilemap mapTilemap = FindSetByLayer(TileLayer.Map).Tilemap;

            Vector3Int cellPosition = mapTilemap.WorldToCell(worldPosition);


            return null;
        }

        public void SetTileBaseToLayer(TileLayer layer, TileBase tileBase, Tile tile)
        {
            Assert.IsNotNull(tileBase);
            Assert.That(ContainsTile(tile));

            Assert.That(TryGetTilemapOnLayer(layer, out Tilemap tilemap));

            tilemap.SetTile(tile.LocationInTileMap, tileBase);
            OnTileBaseChanged?.Invoke(tile, layer);
        }

        public bool TryGetTilemapOnLayer(TileLayer layer, out Tilemap tilemap)
        {
            tilemap = FindSetByLayer(layer).Tilemap;

            return tilemap != null;
        }

        public bool ContainsTile(Tile toCheck)
            => toCheck != null &&
            _tiles.Contains(toCheck);

        private void Validate(TilemapLayerSet[] maps)
        {
            IEnumerable<IGrouping<TileLayer, TilemapLayerSet>> groupsByLayer = maps.GroupBy(set => set.Layer);

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

            Vector3Int maxCellsPosition = mapCellBounds.max - Vector3Int.one;
            Vector3Int minCellsPosition = mapCellBounds.min;

            for (int x = minCellsPosition.x; x <= maxCellsPosition.x; x++)
            {
                for (int y = minCellsPosition.y; y <= maxCellsPosition.y; y++)
                {
                    for (int z = minCellsPosition.z; z <= maxCellsPosition.z; z++)
                    {
                        var currentPosition = new Vector3Int(x, y, z);

                        TileData tileData = new();
                        mapTilemap.GetTile(currentPosition).GetTileData(currentPosition, mapTilemap, ref tileData);

                        if (tileData.sprite != null)
                            _tiles.Add(new Tile(this, currentPosition));
                    }
                }
            }
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
