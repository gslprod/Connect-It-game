using ConnectIt.Gameplay.Model;
using ConnectIt.Gameplay.Observers;
using ConnectIt.Input;
using ConnectIt.Localization;
using ConnectIt.Shop.Customer;
using ConnectIt.Shop.Goods.Boosts;
using ConnectIt.UI.DialogBox;
using System.Collections;
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
        private ICustomer _customer;
        private SkipLevelBoost.Factory _skipLevelBoostFactory;

        [Inject]
        public void Constructor(
            IGameStateObserver gameStateObserver,
            DialogBoxView.Factory dbFactory,
            TextKey.Factory textKeyFactory,
            ICustomer customer,
            SkipLevelBoost.Factory skipLevelBoostFactory)
        {
            _gameStateObserver = gameStateObserver;
            _dbFactory = dbFactory;
            _textKeyFactory = textKeyFactory;
            _customer = customer;
            _skipLevelBoostFactory = skipLevelBoostFactory;
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

            _customer.Storage.AddItem(_skipLevelBoostFactory.Create());
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