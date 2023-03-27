﻿using ConnectIt.Gameplay.Model;
using UnityEngine;

namespace ConnectIt.Gameplay.MonoWrappers
{
    public class TilemapsMonoWrapper : MonoBehaviour
    {
        public TilemapLayerSet[] TilemapLayers => _tilemapLayers;
        public TileBaseAndObjectInfoSet[] ObjectsInfo => _objectsInfo;

        [SerializeField] private TilemapLayerSet[] _tilemapLayers;
        [SerializeField] private TileBaseAndObjectInfoSet[] _objectsInfo;
    }
}