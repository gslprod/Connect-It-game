using ConnectIt.Infrastructure.Setters;
using ConnectIt.Input.GameplayInputRouterStates;
using ConnectIt.Utilities;
using System;
using UnityEngine.InputSystem;
using Zenject;

namespace ConnectIt.Input
{
    public class GameplayInputRouter : IInitializable, ITickable
    {
        public event Action StateChanged;

        public bool Enabled => _input.asset.enabled;
        public BaseState State { get; private set; }

        private readonly GameplayInput _input;
        private readonly IdleTilemapsInteractionState.Factory _idleStateFactory;

        private PriorityAwareSetter<bool> _enableSetter;

        public GameplayInputRouter(
            GameplayInput gameplayInput,
            IdleTilemapsInteractionState.Factory idleStateFactory)
        {
            _input = gameplayInput;
            _idleStateFactory = idleStateFactory;
        }

        public void Initialize()
        {
            ResetState();

            _enableSetter = new(
                SetEnableInternal,
                () => Enabled,
                true);
        }

        public void Tick()
        {
            if (!_input.asset.enabled)
                return;

            State.Update();
        }

        public void SetEnable(bool enable, int priority)
        {
            _enableSetter.SetValue(enable, priority);
        }

        public void ResetEnableWithPriority(int priority)
        {
            _enableSetter.ResetValueWithPriority(priority);
        }

        public void SetState(BaseState newState)
        {
            Assert.ArgIsNotNull(newState);

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

        private void SetEnableInternal(bool enable)
        {
            if (enable)
                Enable();
            else
                Disable();
        }

        private void Enable()
        {
            _input.Enable();

            SubscribeToInput();
        }

        private void Disable()
        {
            _input.Disable();

            UnsubscribeFromInput();
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
