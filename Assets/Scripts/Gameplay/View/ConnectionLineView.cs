using ConnectIt.Model;
using System.Linq;
using UnityEngine;
using Zenject;

namespace ConnectIt.View
{
    public class ConnectionLineView : MonoBehaviour
    {
        private ConnectionLine _connectionLineModel;
        private LineRenderer _lineRenderer;

        [Inject]
        public void Constructor(ConnectionLine model)
        {
            _connectionLineModel = model;
        }

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void OnEnable()
        {
            if (_connectionLineModel == null)
                return;

            _connectionLineModel.UsingTilesChanged += OnUsingTilesChanged;

            UpdateView();
        }

        private void OnDisable()
        {
            _connectionLineModel.UsingTilesChanged += OnUsingTilesChanged;
        }

        private void UpdateView()
        {
            int usingTilesCount = _connectionLineModel.UsingTiles.Count();

            _lineRenderer.positionCount = usingTilesCount;

            for (int i = 0; i < _connectionLineModel.UsingTiles.Count(); i++)
            {
                Vector3 position = _connectionLineModel.UsingTiles[i].Tile.GetWorldPosition();

                _lineRenderer.SetPosition(i, position);
            }
        }

        private void OnUsingTilesChanged(ConnectionLine line)
        {
            UpdateView();
        }

        public class Factory : PlaceholderFactory<ConnectionLine, ConnectionLineView> { }
    }
}
