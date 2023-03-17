using System.Collections.Generic;
using UnityEngine;

namespace ConnectIt.Model
{
    public class Tile
    {
        public Vector2 LocationInMap { get; }

        private List<TileUser> _users;

        public Tile(Vector2 locationInMap)
        {
            LocationInMap = locationInMap;
        }

        public bool TryAddUser(TileUser toAdd)
        {
            if (toAdd == null)
                return false;

            bool canBeAdded = _users.TrueForAll(user => user.CanBeOtherUserAdded(toAdd));
            if (canBeAdded)
                _users.Add(toAdd);

            return canBeAdded;
        }

        public bool TryRemoveUser(TileUser toRemove)
        {
            if (!ContainsUser(toRemove))
                return false;

            bool canBeRemoved = toRemove.CanBeRemoved;
            if (canBeRemoved)
                _users.Remove(toRemove);

            return canBeRemoved;
        }

        public bool ContainsUser(TileUser toCheck)
            => _users.Contains(toCheck);
    }
}
