using ConnectIt.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace ConnectIt.Model
{
    public enum TileLayer
    {
        Map,
        Main,
        Line
    }

    public class Tile
    {
        public Vector2 LocationInTileMap { get; }

        private readonly List<TileUser> _users = new();
        private readonly TileAndLayerSet[] _tilesAndLayers;

        public Tile(TileAndLayerSet[] tilesAndLayers)
        {
            Validate(tilesAndLayers);

            _tilesAndLayers = tilesAndLayers;
        }

        public TileBase GetTileBase(TileLayer layer)
        {
            TileAndLayerSet found = FindSetByLayer(layer);
            Assert.IsNotNull(found);

            return found.Tile;
        }

        public bool TryGetTileBase(TileLayer layer, out TileBase tile)
        {
            TileAndLayerSet found = FindSetByLayer(layer);

            tile = found?.Tile;
            return tile != null;
        }

        public void AddUser(TileUser toAdd)
        {
            Assert.That(CanUserBeAdded(toAdd));

            _users.Add(toAdd);
        }

        public void RemoveUser(TileUser toRemove)
        {
            Assert.That(CanUserBeRemoved(toRemove));

            _users.Remove(toRemove);
        }

        public bool CanUserBeAdded(TileUser user)
        {
            return
                user != null &&
                UserInLayerExists(user.Layer);
        }

        public bool CanUserBeRemoved(TileUser user)
        {
            return
                ContainsUser(user);
        }

        public bool ContainsUser(TileUser toCheck)
            => toCheck != null &&
            _users.Contains(toCheck);

        public bool UserInLayerExists(TileLayer layer)
            => _users.Exists(user => user.Layer == layer);

        private TileAndLayerSet FindSetByLayer(TileLayer layer)
            => _tilesAndLayers.First(set => set.Layer == layer);

        private void Validate(TileAndLayerSet[] tilesAndLayers)
        {
            IEnumerable<IGrouping<TileLayer, TileAndLayerSet>> groupsByLayer = tilesAndLayers.GroupBy(set => set.Layer);
            
            int groupsWithDuplicateLayersCount =
                groupsByLayer.Where(group => group.Count() > 1)
                .Count();

            bool containsMapLayer = groupsByLayer.Any(group => group.Key == TileLayer.Map);

            Assert.That(groupsWithDuplicateLayersCount == 0 &&
                containsMapLayer);
        }
    }

    public class TileAndLayerSet
    {
        public TileBase Tile { get; }
        public TileLayer Layer { get; }

        public TileAndLayerSet(TileBase tile, TileLayer layer)
        {
            Tile = tile;
            Layer = layer;
        }
    }
}
