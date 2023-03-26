using ConnectIt.Config;
using ConnectIt.Model;
using System.Linq;
using UnityEngine;
using Zenject;

namespace ConnectIt.View
{
    public class ConnectionLineView : MonoBehaviour
    {
        private ConnectionLine _connectionLineModel;
        private GameplayViewConfig _gameplayViewConfig;

        private LineRenderer _lineRenderer;

        [Inject]
        public void Constructor(ConnectionLine model,
            GameplayViewConfig gameplayViewConfig)
        {
            _connectionLineModel = model;
            _gameplayViewConfig = gameplayViewConfig;
        }

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Start()
        {
            _connectionLineModel.Disposing += OnModelDisposing;

            UpdateColor();
        }

        private void OnEnable()
        {
            _connectionLineModel.UsingTilesChanged += OnUsingTilesChanged;

            UpdateView();
        }

        private void OnDisable()
        {
            _connectionLineModel.UsingTilesChanged += OnUsingTilesChanged;
        }

        private void OnDestroy()
        {
            _connectionLineModel.Disposing -= OnModelDisposing;
        }

        private void UpdateView()
        {
            UpdateLineVerticles();
        }

        private void UpdateLineVerticles()
        {
            int usingTilesCount = _connectionLineModel.UsingTiles.Count();

            _lineRenderer.positionCount = usingTilesCount;

            for (int i = 0; i < _connectionLineModel.UsingTiles.Count(); i++)
            {
                Vector3 position = _connectionLineModel.UsingTiles.ElementAt(i).Tile.GetWorldPosition();

                _lineRenderer.SetPosition(i, position);
            }
        }

        private void UpdateColor()
        {
            Color mainColor = _gameplayViewConfig.GetColorByCompatibilityIndex(_connectionLineModel.CompatibilityIndex);

            _lineRenderer.startColor = mainColor;
            _lineRenderer.endColor = mainColor;
        }

        private void OnUsingTilesChanged(ConnectionLine line)
        {
            UpdateView();
        }

        private void OnModelDisposing(ConnectionLine obj)
        {
            Destroy(gameObject);
        }

        public class Factory : PlaceholderFactory<ConnectionLine, ConnectionLineView> { }
    }
}
