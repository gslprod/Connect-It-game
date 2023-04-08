using ConnectIt.Gameplay;
using ConnectIt.Gameplay.Model;
using ConnectIt.Gameplay.Observers;
using ConnectIt.Infrastructure.CreatedObjectNotifiers;
using ConnectIt.Input;
using ConnectIt.Localization;
using ConnectIt.UI.DialogBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
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

        private UIDocument _uiDoc;
        private DialogBoxView.Factory _dbFactory;
        private TextKey.Factory _textKeyFactory;

        private IGameStateObserver _gameStateObserver;

        [Inject]
        public void Constructor(
            IGameStateObserver gameStateObserver,
            DialogBoxView.Factory dbFactory,
            TextKey.Factory textKeyFactory)
        {
            _gameStateObserver = gameStateObserver;
            _dbFactory = dbFactory;
            _textKeyFactory = textKeyFactory;
        }

        private void Start()
        {
            //_uiDoc = FindObjectOfType<UIDocument>();

            //var parent = _uiDoc.rootVisualElement.Q<VisualElement>("root");
            //DialogBoxCreationData dialogBoxCreationData = new(parent,
            //    _textKeyFactory.Create(TextKeysConstants.DialogBox.QuitLevelConfirm_Title, null),
            //    _textKeyFactory.Create(TextKeysConstants.DialogBox.QuitLevelConfirm_Message, null),
            //    new DialogBoxButtonInfo[] {
            //        new DialogBoxButtonInfo(
            //        _textKeyFactory.Create(TextKeysConstants.DialogBox.QuitLevelConfirm_Title, null), null, DialogBoxButtonType.Accept, true) });

            //StartCoroutine(Delay(dialogBoxCreationData));
        }

        private IEnumerator Delay(DialogBoxCreationData dialogBoxCreationData)
        {
            yield return new WaitForSeconds(1);
            _dbFactory.Create(dialogBoxCreationData);
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