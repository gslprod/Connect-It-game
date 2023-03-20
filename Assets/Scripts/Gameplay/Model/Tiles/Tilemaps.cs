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
        private Tile[] _tiles;

        private Dictionary<int, int> _xCoordinateArrayPointers = new();

        public Tilemaps(TilemapLayerSet[] maps)
        {
            Validate(maps);
            _maps = maps;

            CreateTiles();
        }

        public Tile GetTileAtWorldPosition(Vector3 worldPosition)
        {
            Assert.That(TryGetTileAtWorldPosition(worldPosition, out Tile found));

            return found;
        }

        public bool TryGetTileAtWorldPosition(Vector3 worldPosition, out Tile tile)
        {
            Tilemap mapTilemap = FindSetByLayer(TileLayer.Map).Tilemap;

            Vector3Int cellPosition = mapTilemap.WorldToCell(worldPosition);
            tile = FindTileAtCellPosition(cellPosition);

            return tile != null;
        }

        public bool HasTileAtWorldPosition(Vector3 worldPosition)
        {
            return TryGetTileAtWorldPosition(worldPosition, out Tile _);
        }

        public bool ContainsTile(Tile toCheck)
        {
            Assert.IsNotNull(toCheck);

            return FastFindTileByXCoordinate(toCheck.LocationInTileMap.x, tile => tile == toCheck) != null;
        }

        public void SetTileBaseOnLayer(TileLayer layer, TileBase tileBase, Tile tile)
        {
            Assert.IsNotNull(tileBase);
            Assert.That(ContainsTile(tile));

            Assert.That(TryGetTilemapOnLayer(layer, out Tilemap tilemap));

            tilemap.SetTile(tile.LocationInTileMap, tileBase);
            OnTileBaseChanged?.Invoke(tile, layer);
        }

        public T GetTileOnLayer<T>(TileLayer layer, Tile tile) where T : TileBase
        {
            Assert.IsNotNull(tile);

            Assert.That(TryGetTilemapOnLayer(layer, out Tilemap tilemap));

            return tilemap.GetTile<T>(tile.LocationInTileMap);
        }

        private void Validate(TilemapLayerSet[] maps)
        {
            IEnumerable<IGrouping<TileLayer, TilemapLayerSet>> groupsByLayer = maps.GroupBy(set => set.Layer);

            int groupsWithDuplicateLayersCount =
                groupsByLayer.Where(group => group.Count() > 1)
                .Count();

            bool containsMapLayer = groupsByLayer.Any(group => group.Key == TileLayer.Map);

            Assert.That(groupsWithDuplicateLayersCount == 0,
                containsMapLayer);
        }

        private void CreateTiles()
        {
            Tilemap mapTilemap = FindSetByLayer(TileLayer.Map).Tilemap;

            mapTilemap.CompressBounds();
            BoundsInt mapCellBounds = mapTilemap.cellBounds;

            Vector3Int boundsSize = mapCellBounds.size;
            Vector3Int maxCellsPosition = mapCellBounds.max - Vector3Int.one;
            Vector3Int minCellsPosition = mapCellBounds.min;

            _tiles = new Tile[boundsSize.x * boundsSize.y * boundsSize.z];
            int index = 0;

            for (int x = minCellsPosition.x; x <= maxCellsPosition.x; x++)
            {
                _xCoordinateArrayPointers.Add(x, index);

                for (int y = minCellsPosition.y; y <= maxCellsPosition.y; y++)
                {
                    for (int z = minCellsPosition.z; z <= maxCellsPosition.z; z++)
                    {
                        var currentCellPosition = new Vector3Int(x, y, z);

                        TileData tileData = new();
                        mapTilemap.GetTile(currentCellPosition).GetTileData(currentCellPosition, mapTilemap, ref tileData);

                        if (tileData.sprite != null)
                            _tiles[index++] = new Tile(this, currentCellPosition);
                    }
                }
            }

            Array.Resize(ref _tiles, index);
        }

        private Tile FindTileAtCellPosition(Vector3Int cellPosition)
        {
            return FastFindTileByXCoordinate(cellPosition.x, tile => tile.LocationInTileMap == cellPosition);
        }

        private Tile FastFindTileByXCoordinate(int xCoordinate, Func<Tile, bool> condition)
        {
            int xStartIndex = _xCoordinateArrayPointers[xCoordinate];
            if (!_xCoordinateArrayPointers.TryGetValue(xCoordinate + 1, out int xLastIndex))
                xLastIndex = _tiles.Length;

            for (int i = xStartIndex; i < xLastIndex; i++)
            {
                Tile current = _tiles[i];

                if (condition(current))
                    return current;
            }

            return null;
        }

        private bool TryGetTilemapOnLayer(TileLayer layer, out Tilemap tilemap)
        {
            tilemap = FindSetByLayer(layer).Tilemap;

            return tilemap != null;
        }

        private TilemapLayerSet FindSetByLayer(TileLayer layer)
            => _maps.First(set => set.Layer == layer);
    }

    [Serializable]
    public class TilemapLayerSet
    {
        public Tilemap Tilemap => _tilemap;
        public TileLayer Layer => _layer;

        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TileLayer _layer;

        public TilemapLayerSet(Tilemap tilemap, TileLayer layer)
        {
            _tilemap = tilemap;
            _layer = layer;
        }
    }
}
