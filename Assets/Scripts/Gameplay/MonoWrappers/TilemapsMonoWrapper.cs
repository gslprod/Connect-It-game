using ConnectIt.Gameplay.Model;
using UnityEngine;
using Zenject;

namespace ConnectIt.Gameplay.MonoWrappers
{
    public class TilemapsMonoWrapper : MonoBehaviour
    {
        public TilemapLayerSet[] TilemapLayers => _tilemapLayers;
        public TileBaseAndObjectInfoSet[] ObjectsInfo => _objectsInfo;
        public int TargetLevel => _targetLevel;

        [SerializeField] private TilemapLayerSet[] _tilemapLayers;
        [SerializeField] private TileBaseAndObjectInfoSet[] _objectsInfo;
        [SerializeField] private int _targetLevel;

        public class Factory : PlaceholderFactory<TilemapsMonoWrapper, TilemapsMonoWrapper> { }
    }
}