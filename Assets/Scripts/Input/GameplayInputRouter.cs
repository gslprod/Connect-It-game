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

        public BaseState State { get; private set; }

        private readonly GameplayInput _input;
        private readonly IFactory<IdleTilemapsInteractionState> _idleStateFactory;

        public GameplayInputRouter(
            GameplayInput gameplayInput,
            IFactory<IdleTilemapsInteractionState> idleStateFactory)
        {
            _input = gameplayInput;
            _idleStateFactory = idleStateFactory;
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

        public void SetState(BaseTilemapsInteractionState newState)
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
            SetState(_idleStateFactory.Create());
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
