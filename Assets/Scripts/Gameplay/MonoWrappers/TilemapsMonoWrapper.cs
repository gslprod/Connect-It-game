using ConnectIt.Model;
using UnityEngine;

namespace ConnectIt.MonoWrappers
{
    public class TilemapsMonoWrapper : MonoBehaviour
    {
        public Tilemaps Model
        {
            get
            { 
                if (_model == null)
                    InitModel();

                return _model;
            }
        }

        public TilemapLayerSet[] TilemapLayers => _tilemapLayers;
        public TileBaseAndObjectInfoSet[] ObjectsInfo => _objectsInfo;

        [SerializeField] private TilemapLayerSet[] _tilemapLayers;
        [SerializeField] private TileBaseAndObjectInfoSet[] _objectsInfo;


        private Tilemaps _model;

        private void InitModel()
        {
            _model = new Tilemaps(_tilemapLayers, _objectsInfo);
        }
    }
}