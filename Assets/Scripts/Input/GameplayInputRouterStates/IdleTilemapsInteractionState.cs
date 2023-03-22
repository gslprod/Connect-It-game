﻿using ConnectIt.Infrastructure.CreatedObjectNotifiers;
using ConnectIt.Infrastructure.Factories;
using ConnectIt.Model;
using UnityEngine.InputSystem;
using static ConnectIt.Model.ConnectionLine;
using static ConnectIt.Model.Port;

namespace ConnectIt.Input.GameplayInputRouterStates
{
    public class IdleTilemapsInteractionState : BaseTilemapsInteractionState
    {
        private readonly IFactory<CreatingConnectionLineState> _creatingLineStateFactory;
        private readonly IFactory<RemovingConnectionLineState> _removingLineStateFactory;
        private readonly ICreatedObjectNotifier<ConnectionLine> _createdConnectionLineNotifier;

        public IdleTilemapsInteractionState(
            GameplayInput input,
            GameplayInputRouter inputRouter,
            RenderCameraProvider cameraProvider,
            Tilemaps tilemaps,
            IFactory<CreatingConnectionLineState> creatingLineStateFactory,
            IFactory<RemovingConnectionLineState> removingLineStateFactory,
            ICreatedObjectNotifier<ConnectionLine> createdConnectionLineNotifier) : base(input, inputRouter, cameraProvider, tilemaps)
        {
            _creatingLineStateFactory = creatingLineStateFactory;
            _removingLineStateFactory = removingLineStateFactory;
            _createdConnectionLineNotifier = createdConnectionLineNotifier;
        }

        public override void OnInteractionPressPerformed(InputAction.CallbackContext context)
        {
            if (!TryGetTileByPositionOnScreen(out Tile interactedTile))
                return;

            if (TrySetCreatingLineState(interactedTile))
                return;

            if (TrySetRemovingLineState(interactedTile))
                return;
        }

        private bool TrySetCreatingLineState(Tile tile)
        {
            if (!TryGetObjectInfoFromTile(tile, out PortTileUserObjectInfo portInfo))
                return false;

            Port port = portInfo.Port;

            if (port.Connectable.HasConnection)
                return false;

            ConnectionLine createdLine = new(port);
            _createdConnectionLineNotifier.SendNotification(createdLine);

            inputRouter.SetState(_creatingLineStateFactory.Create(new object[] { createdLine }));

            return true;
        }

        private bool TrySetRemovingLineState(Tile tile)
        {
            if (!TryGetObjectInfoFromTile(tile, out ConnectionLineTileUserObjectInfo connectionLineInfo))
                return false;

            inputRouter.SetState(_removingLineStateFactory.Create(new object[] { connectionLineInfo.ConnectionLine }));

            return true;
        }
    }
}
