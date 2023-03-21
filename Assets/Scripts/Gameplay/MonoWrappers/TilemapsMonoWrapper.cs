using ConnectIt.Model;
using UnityEngine;

namespace ConnectIt.MonoWrappers
{
    public class TilemapsMonoWrapper : MonoBehaviour
    {
        [SerializeField] private TilemapLayerSet[] _tilemapLayers;
        [SerializeField] private TileBaseAndObjectInfoSet[] _objectsInfo;

        private Tilemaps _model;

        private void Awake()
        {
            InitModel();
        }

        private void InitModel()
        {
            _model = new Tilemaps(_tilemapLayers, _objectsInfo);
        }
    }
}