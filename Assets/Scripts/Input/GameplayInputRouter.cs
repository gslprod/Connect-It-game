using ConnectIt.Infrastructure.Factories;
using ConnectIt.Input.GameplayInputRouterStates;
using ConnectIt.Utilities;
using System;
using UnityEngine.InputSystem;

namespace ConnectIt.Input
{
    public class GameplayInputRouter
    {
        public event Action StateChanged;

        public GameplayInputRouterState State { get; private set; }

        private readonly GameplayInput _input;
        private readonly IFactory<ConnectionsGameplayInputRouterState> _connectionsStateFactory;

        public GameplayInputRouter(
            GameplayInput gameplayInput,
            IFactory<ConnectionsGameplayInputRouterState> connectionsStateFactory)
        {
            _input = gameplayInput;
            _connectionsStateFactory = connectionsStateFactory;

            ResetState();
            //to remove
            Enable();
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
            State.Update();
        }

        public void SetState(GameplayInputRouterState newState)
        {
            Assert.IsNotNull(newState);

            if (State == newState)
                return;

            State?.StateExit();
            State = newState;

            StateChanged?.Invoke();
        }

        public void ResetState()
        {
            SetState(_connectionsStateFactory.Create());
        }

        private void OnInteractionClick(InputAction.CallbackContext context)
        {
            State.OnInteractionClick(context);
        }

        private void OnInteractionPressPerformed(InputAction.CallbackContext context)
        {
            State.OnInteractionPressPerformed(context);
        }

        private void OnInteractionPressCanceled(InputAction.CallbackContext context)
        {
            State.OnInteractionPressCanceled(context);
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
