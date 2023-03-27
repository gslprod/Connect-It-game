using ConnectIt.Gameplay.Model;
using ConnectIt.Utilities.Extensions;
using System.Linq;
using UnityEngine.InputSystem;
using Zenject;
using static ConnectIt.Gameplay.Model.Port;

namespace ConnectIt.Input.GameplayInputRouterStates
{
    public class CreatingConnectionLineState : BaseTilemapsInteractionState
    {
        private readonly ConnectionLine _creatableLine;

        public CreatingConnectionLineState(
            GameplayInput input,
            GameplayInputRouter inputRouter,
            RenderCameraProvider cameraProvider,
            Tilemaps tilemaps,
            ConnectionLine createdLine) : base(input, inputRouter, cameraProvider, tilemaps)
        {
            _creatableLine = createdLine;
        }

        public override void OnInteractionPressCanceled(InputAction.CallbackContext context)
        {
            inputRouter.ResetState();
        }

        public override void Update()
        {
            if (!TryGetTileByPositionOnScreen(out Tile interactedTile))
                return;

            if (lastOnUpdateInteractedTile == interactedTile)
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

            Port port = portInfo.Port;

            if (!_creatableLine.CanBeCompletedWith(port))
                return;

            _creatableLine.CompleteConnection(port);

            inputRouter.ResetState();
        }

        public override void StateExit()
        {
            base.StateExit();

            if (_creatableLine.ConnectionCompleted)
                return;

            _creatableLine.Dispose();
        }

        public class Factory : PlaceholderFactory<ConnectionLine, CreatingConnectionLineState> { }
    }
}
