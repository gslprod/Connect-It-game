﻿using ConnectIt.Infrastructure.Dispose;
using ConnectIt.Utilities;
using ConnectIt.Utilities.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ConnectIt.Gameplay.Model
{
    public enum TileLayer
    {
        Map,
        Main,
        Line
    }

    public class Tile : IDisposeNotifier<Tile>
    {
        public event Action<Tile, TileLayer> UsersChanged;
        public event Action<Tile, TileLayer> TileBaseChanged;
        public event Func<object> TileUsersInfoRequest;
        public event Action<Tile> Disposing;

        public Vector3Int LocationInTileMap { get; }

        private readonly Tilemaps _tilemaps;
        private readonly List<TileUser> _users = new();

        public Tile(Tilemaps tilemaps, Vector3Int locationInTileMap)
        {
            _tilemaps = tilemaps;
            LocationInTileMap = locationInTileMap;

            tilemaps.OnTileBaseChanged += OnTileBaseChanged;
        }

        public object[] RequestUsersInfo()
        {
            return TileUsersInfoRequest?.InvokeAllAndGetResults();
        }

        public void SetTileBaseOnLayer(TileLayer layer, TileBase tileBase)
        {
            _tilemaps.SetTileBaseOnLayer(layer, tileBase, this);
        }

        public T GetTileOnLayer<T>(TileLayer layer) where T : TileBase
        {
            return _tilemaps.GetTileOnLayer<T>(layer, this);
        }

        public Vector3 GetWorldPosition()
        {
            return _tilemaps.GetWorldPositionOfTile(this);
        }

        public void AddUser(TileUser toAdd)
        {
            Assert.That(CanUserBeAdded(toAdd));

            _users.Add(toAdd);
            UsersChanged?.Invoke(this, toAdd.Layer);
        }

        public void RemoveUser(TileUser toRemove)
        {
            Assert.That(CanUserBeRemoved(toRemove));

            _users.Remove(toRemove);
            UsersChanged?.Invoke(this, toRemove.Layer);
        }

        public bool CanUserBeAdded(TileUser user)
        {
            return
                user != null &&
                !UserInLayerExists(user.Layer);
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

        public void Dispose()
        {
            _tilemaps.OnTileBaseChanged -= OnTileBaseChanged;
            Disposing?.Invoke(this);
        }

        private void OnTileBaseChanged(Tile tile, TileLayer layer)
        {
            if (this != tile)
                return;

            TileBaseChanged?.Invoke(this, layer);
        }
    }
}