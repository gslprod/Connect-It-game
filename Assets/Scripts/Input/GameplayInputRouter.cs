using ConnectIt.Input.GameplayInputRouterStates;
using ConnectIt.Utilities;
using System;
using UnityEngine.InputSystem;
using Zenject;
using Factories = ConnectIt.Infrastructure.Factories;

namespace ConnectIt.Input
{
    public class GameplayInputRouter : IInitializable, ITickable
    {
        public event Action StateChanged;

        public BaseState State { get; private set; }

        private readonly GameplayInput _input;
        private readonly Factories.IFactory<IdleTilemapsInteractionState> _idleStateFactory;

        public GameplayInputRouter(
            GameplayInput gameplayInput,
            Factories.IFactory<IdleTilemapsInteractionState> idleStateFactory)
        {
            _input = gameplayInput;
            _idleStateFactory = idleStateFactory;
        }

        public void Initialize()
        {
            ResetState();

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

        public void Tick()
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
