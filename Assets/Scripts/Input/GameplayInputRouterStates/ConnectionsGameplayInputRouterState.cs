using ConnectIt.Infrastructure.CreatedObjectNotifiers;
using ConnectIt.Model;
using ConnectIt.Utilities.Extensions;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static ConnectIt.Model.Port;

namespace ConnectIt.Input.GameplayInputRouterStates
{
    public class ConnectionsGameplayInputRouterState : GameplayInputRouterState
    {
        public bool CreatingConnectionLine => _creatableLine != null;

        private readonly GameplayInput _input;
        private readonly RenderCameraProvider _cameraProvider;
        private readonly Tilemaps _tilemaps;
        private readonly ICreatedObjectNotifier<ConnectionLine> _createdConnectionLineNotifier;

        private ConnectionLine _creatableLine;
        private Tile _lastOnUpdateInteractedTile;

        public ConnectionsGameplayInputRouterState(
            GameplayInput input,
            RenderCameraProvider cameraProvider,
            Tilemaps tilemaps,
            ICreatedObjectNotifier<ConnectionLine> createdConnectionLineNotifier)
        {
            _input = input;
            _cameraProvider = cameraProvider;
            _tilemaps = tilemaps;
            _createdConnectionLineNotifier = createdConnectionLineNotifier;
        }

        public override void OnInteractionPressPerformed(InputAction.CallbackContext context)
        {
            TryCreateConnectionLine();
        }

        public override void OnInteractionPressCanceled(InputAction.CallbackContext context)
        {
            TryRemoveConnectionLineIfNotCompleted();
        }

        public override void Update()
        {
            if (CreatingConnectionLine)
                ConnectionLineExpanding();
            else
                ConnectionLineRemoving();
        }

        public override void StateExit()
        {
            TryRemoveConnectionLineIfNotCompleted();
        }

        private void TryCreateConnectionLine()
        {
            if (CreatingConnectionLine)
                return;

            if (!TryGetTileByPositionOnScreen(out Tile interactedTile))
                return;

            if (!TryGetObjectInfoFromTile(interactedTile, out PortTileUserObjectInfo portInfo))
                return;

            _creatableLine = new ConnectionLine(portInfo.Port);
            _createdConnectionLineNotifier.SendNotification(_creatableLine);
        }

        private void TryRemoveConnectionLineIfNotCompleted()
        {
            if (!CreatingConnectionLine)
                return;

            if (_creatableLine.ConnectionCompleted)
                return;

            _creatableLine.Remove();
            _creatableLine = null;
        }

        private void ConnectionLineExpanding()
        {
            if (!TryGetTileByPositionOnScreen(out Tile interactedTile))
                return;

            if (_lastOnUpdateInteractedTile == interactedTile)
                return;

            SetLastOnUpdateInteractedTile(interactedTile);

            Tile lastUsedTileByLine = _creatableLine.UsingTiles.Last().Tile;
            if (!lastUsedTileByLine.IsNearTo(interactedTile, 1))
                return;

            if (interactedTile.UserInLayerExists(TileLayer.Line))
                return;

            if (!TryGetObjectInfoFromTile(interactedTile, out PortTileUserObjectInfo portInfo))
            {
                _creatableLine.ExpandLine(interactedTile);
                return;
            }

            _creatableLine.CompleteConnection(portInfo.Port);
            _creatableLine = null;
        }

        private void ConnectionLineRemoving()
        {

        }

        private bool TryGetTileByPositionOnScreen(out Tile tile)
        {
            Vector2 positionOnScreen = _input.Main.InteractionPosition.ReadValue<Vector2>();
            Vector3 worldPosition = _cameraProvider.RenderCamera.ScreenToWorldPoint(positionOnScreen);

            return _tilemaps.TryGetTileAtWorldPosition(worldPosition, out tile);
        }

        private bool TryGetObjectInfoFromTile<T>(Tile tile, out T objInfo) where T : class
        {
            objInfo = null;

            var tileUsersInfo = tile.RequestUsersInfo();
            if (tileUsersInfo == null || tileUsersInfo.Length == 0)
                return false;

            foreach (var userInfo in tileUsersInfo)
            {
                if (userInfo is T info)
                {
                    objInfo = info;
                    break;
                }
            }

            return objInfo != null;
        }

        private void SetLastOnUpdateInteractedTile(Tile tile)
        {
            _lastOnUpdateInteractedTile = tile;
            _lastOnUpdateInteractedTile.UsersChanged += OnLastInteractedTileUserChanged;
        }

        private void ResetLastOnUpdateInteractedTile()
        {
            _lastOnUpdateInteractedTile.UsersChanged -= OnLastInteractedTileUserChanged;
            _lastOnUpdateInteractedTile = null;
        }

        private void OnLastInteractedTileUserChanged(Tile tile)
        {
            ResetLastOnUpdateInteractedTile();
        }
    }
}
