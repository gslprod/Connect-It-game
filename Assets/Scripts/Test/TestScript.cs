using ConnectIt.Gameplay;
using ConnectIt.Gameplay.Model;
using ConnectIt.Gameplay.Observers;
using ConnectIt.Infrastructure.CreatedObjectNotifiers;
using ConnectIt.Input;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace ConnectIt.Test
{
    public class TestScript : MonoBehaviour
    {
        [SerializeField] private bool _skip;
        [SerializeField] private TileBase _tile1;
        [SerializeField] private TileBase _tile2;

        private GameplayInput _input;
        private Tilemap _tilemap;
        private Camera _camera;

        private IGameStateObserver _gameStateObserver;

        [Inject]
        public void Constructor(
            IGameStateObserver gameStateObserver)
        {
            _gameStateObserver = gameStateObserver;
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            //print(_gameStateObserver.GameCompleteProgressPercents);

            //if (_connectionLine != null && _connectionLine.UsingTiles.Last().Tile != null)
            //{
            //    print(_connectionLine.UsingTiles.Last().Tile.LocationInTileMap);
            //}
        }

        private void InteractionPosition_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (_skip)
                return;

            Vector2 interactionPosition = obj.ReadValue<Vector2>();
            Vector3 worldPosition = _camera.ScreenToWorldPoint(interactionPosition);
            Vector3Int selectedCell = _tilemap.WorldToCell(worldPosition);

            print(_tilemap.GetTile(selectedCell) == null);
        }
    }
}