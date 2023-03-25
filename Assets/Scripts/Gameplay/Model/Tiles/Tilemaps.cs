using ConnectIt.Infrastructure.CreatedObjectNotifiers;
using ConnectIt.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;
using static ConnectIt.Model.TileBaseAndObjectInfoSet;

namespace ConnectIt.Model
{
    public class Tilemaps : IInitializable, IDisposable
    {
        public event Action<Tile, TileLayer> OnTileBaseChanged;

        private readonly TilemapLayerSet[] _maps;
        private readonly TileBaseAndObjectInfoSet[] _spriteAndObjectTypeSets;
        private readonly ICreatedObjectNotifier<Port> _createdPortNotifier;

        private readonly Dictionary<int, int> _xCoordinateArrayPointers = new();
        private Tile[] _tiles;

        public Tilemaps(TilemapLayerSet[] maps,
            TileBaseAndObjectInfoSet[] spriteAndObjectTypeSets,
            ICreatedObjectNotifier<Port> createdPortNotifier)
        {
            Validate(maps);
            _maps = maps;

            Validate(spriteAndObjectTypeSets);
            _spriteAndObjectTypeSets = spriteAndObjectTypeSets;

            _createdPortNotifier = createdPortNotifier;
        }

        public void Initialize()
        {
            CreateTiles();
            CreateObjectsOnMap();
        }

        public Tile GetTileAtWorldPosition(Vector3 worldPosition)
        {
            Assert.That(TryGetTileAtWorldPosition(worldPosition, out Tile found));

            return found;
        }

        public bool TryGetTileAtWorldPosition(Vector3 worldPosition, out Tile tile)
        {
            Tilemap mapTilemap = GetTilemapOnLayer(TileLayer.Map);

            Vector3Int cellPosition = mapTilemap.WorldToCell(worldPosition);
            tile = FindTileAtCellPosition(cellPosition);

            return tile != null;
        }

        public Vector3 GetWorldPositionOfTile(Tile tile)
        {
            Assert.That(ContainsTile(tile));

            Tilemap mapTilemap = GetTilemapOnLayer(TileLayer.Map);

            Vector3 tilePosition = mapTilemap.CellToWorld(tile.LocationInTileMap);
            tilePosition += mapTilemap.cellSize / 2;

            return tilePosition;
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

            Tilemap tilemap = GetTilemapOnLayer(layer);

            tilemap.SetTile(tile.LocationInTileMap, tileBase);
            OnTileBaseChanged?.Invoke(tile, layer);
        }

        public T GetTileOnLayer<T>(TileLayer layer, Tile tile) where T : TileBase
        {
            Assert.IsNotNull(tile);
            Assert.That(ContainsTile(tile));

            Tilemap tilemap = GetTilemapOnLayer(layer);

            return tilemap.GetTile<T>(tile.LocationInTileMap);
        }

        public void Dispose()
        {
            foreach (var tile in _tiles)
            {
                tile.Dispose();
            }
        }

        private void Validate(TilemapLayerSet[] maps)
        {
            Assert.IsNotNull(maps);

            IEnumerable<IGrouping<TileLayer, TilemapLayerSet>> groupsByLayer = maps.GroupBy(set => set.Layer);

            int groupsWithDuplicateLayersCount =
                groupsByLayer.Where(group => group.Count() > 1)
                .Count();

            bool containsMapLayer = groupsByLayer.Any(group => group.Key == TileLayer.Map);

            Assert.That(groupsWithDuplicateLayersCount == 0,
                containsMapLayer);
        }

        private void Validate(TileBaseAndObjectInfoSet[] tileBaseAndObjectTypeSets)
        {
            Assert.IsNotNull(tileBaseAndObjectTypeSets);

            var groupsBySprite = tileBaseAndObjectTypeSets.GroupBy(set => set.TileBase);

            int groupsWithDuplicateSpritesCount =
                groupsBySprite.Where(group => group.Count() > 1)
                .Count();

            Assert.That(groupsWithDuplicateSpritesCount == 0);
        }

        private void CreateTiles()
        {
            Tilemap mapTilemap = GetTilemapOnLayer(TileLayer.Map);

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

                        TileBase tileBase = mapTilemap.GetTile(currentCellPosition);

                        if (tileBase == null)
                            continue;

                        _tiles[index++] = new Tile(this, currentCellPosition);
                    }
                }
            }

            Array.Resize(ref _tiles, index);
        }

        private void CreateObjectsOnMap()
        {
            Tilemap mainObjectsTilemap = GetTilemapOnLayer(TileLayer.Main);

            mainObjectsTilemap.CompressBounds();
            BoundsInt mainObjectsCellBounds = mainObjectsTilemap.cellBounds;

            Vector3Int maxCellsPosition = mainObjectsCellBounds.max - Vector3Int.one;
            Vector3Int minCellsPosition = mainObjectsCellBounds.min;

            for (int x = minCellsPosition.x; x <= maxCellsPosition.x; x++)
            {
                for (int y = minCellsPosition.y; y <= maxCellsPosition.y; y++)
                {
                    for (int z = minCellsPosition.z; z <= maxCellsPosition.z; z++)
                    {
                        var currentCellPosition = new Vector3Int(x, y, z);

                        TileBase tileBase = mainObjectsTilemap.GetTile(currentCellPosition);

                        if (tileBase == null)
                            continue;

                        TileBaseAndObjectInfoSet set = _spriteAndObjectTypeSets.First(set => set.TileBase == tileBase);
                        Assert.That(set != null);

                        CreateObjectOnMap(set, currentCellPosition);
                    }
                }
            }
        }

        private void CreateObjectOnMap(TileBaseAndObjectInfoSet infoSet, Vector3Int cellPosition)
        {
            switch (infoSet.ObjectType)
            {
                case SpriteObjectType.Port:
                    CreatePortOnMap(infoSet.PortInfo, cellPosition);
                    break;
                default:
                    Assert.Fail();
                    break;
            }
        }

        private Port CreatePortOnMap(PortObjectInfo info, Vector3Int cellPosition)
        {
            Tile targetTile = FindTileAtCellPosition(cellPosition);

            Port createdPort = new(targetTile, info.CompatibilityIndex);
            _createdPortNotifier.SendNotification(createdPort);

            return createdPort;
        }

        private Tile FindTileAtCellPosition(Vector3Int cellPosition)
        {
            return FastFindTileByXCoordinate(cellPosition.x, tile => tile.LocationInTileMap == cellPosition);
        }

        private Tile FastFindTileByXCoordinate(int xCoordinate, Func<Tile, bool> condition)
        {
            if (!_xCoordinateArrayPointers.TryGetValue(xCoordinate, out int xStartIndex))
                return null;

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
            tilemap = FindSetByLayer(layer)?.Tilemap;

            return tilemap != null;
        }

        private Tilemap GetTilemapOnLayer(TileLayer layer)
        {
            Assert.That(TryGetTilemapOnLayer(layer, out Tilemap tilemap));

            return tilemap;
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

    [Serializable]
    public class TileBaseAndObjectInfoSet
    {
        public enum SpriteObjectType
        {
            Port = 1
        }

        public TileBase TileBase => _tileBase;
        public SpriteObjectType ObjectType => _objectType;
        public PortObjectInfo PortInfo => _portObjectInfo;

        [SerializeField] private TileBase _tileBase;
        [SerializeField] private SpriteObjectType _objectType;
        [SerializeField] private PortObjectInfo _portObjectInfo;

        [Serializable]
        public class PortObjectInfo
        {
            public int CompatibilityIndex => _compatibilityIndex;

            [SerializeField] private int _compatibilityIndex;
        }
    }
}
