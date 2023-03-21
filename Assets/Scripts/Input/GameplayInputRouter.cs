using ConnectIt.Input.GameplayInputRouterStates;
using ConnectIt.Model;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ConnectIt.Input
{
    public class GameplayInputRouter
    {
        private IGameplayInputRouterState _state;

        private readonly GameplayInput _input;
        private readonly RenderCameraProvider _cameraProvider;

        public GameplayInputRouter(GameplayInput gameplayInput,
            RenderCameraProvider cameraProvider)
        {
            _input = gameplayInput;
            _cameraProvider = cameraProvider;
        }

        public void Enable()
        {
            _input.Enable();

            SubscribeToInput();
        }

        public void Disable()
        {
            _input.Disable();

            UnsubscribeFromInput();
        }

        public void Update()
        {

        }

        private void OnInteractionClick(InputAction.CallbackContext context)
        {
            //Vector3 worldPosition = _cameraProvider.Current.ScreenToWorldPoint(positionOnScreen);

            //Tile interactedTile = _tilemaps.GetTileAtWorldPosition(worldPosition);
            //if (interactedTile == null)
            //    return;


        }

        private void OnInteractionPressPerformed(InputAction.CallbackContext context)
        {
            
        }

        private void OnInteractionPressCanceled(InputAction.CallbackContext context)
        {
            
        }

        private void SubscribeToInput()
        {
            _input.Main.InteractionClick.performed += OnInteractionClick;

            _input.Main.InteractionPress.performed += OnInteractionPressPerformed;
            _input.Main.InteractionPress.canceled += OnInteractionPressCanceled;
        }

        private void UnsubscribeFromInput()
        {
            _input.Main.InteractionClick.performed -= OnInteractionClick;

            _input.Main.InteractionPress.performed -= OnInteractionPressPerformed;
            _input.Main.InteractionPress.canceled -= OnInteractionPressCanceled;
        }
    }
}
