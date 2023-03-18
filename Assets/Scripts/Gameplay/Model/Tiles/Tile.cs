using ConnectIt.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectIt.Model
{
    public enum TileLayers
    {
        Main = 1,
        Line
    }

    public class Tile
    {
        public Vector2 LocationInTileMap { get; }

        private List<TileUser> _users = new();

        public Tile(Vector2 locationInTileMap)
        {
            LocationInTileMap = locationInTileMap;
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

        public bool UserInLayerExists(TileLayers layer)
            => _users.Exists(user => user.Layer == layer);
    }
}
