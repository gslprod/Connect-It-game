using ConnectIt.Gameplay.Pause;
using ConnectIt.Infrastructure.Setters;
using ConnectIt.Input.GameplayInputRouterStates;
using ConnectIt.Utilities;
using ConnectIt.Utilities.Extensions.GameplayInputRouter;
using System;
using UnityEngine.InputSystem;
using Zenject;

namespace ConnectIt.Input
{
    public class GameplayInputRouter : IInitializable, ITickable, IDisposable
    {
        public event Action StateChanged;

        public bool Enabled => _input.asset.enabled;
        public BaseState State { get; private set; }

        private readonly GameplayInput _input;
        private readonly IdleTilemapsInteractionState.Factory _idleStateFactory;
        private readonly IPauseService _pauseService;

        private PriorityAwareSetter<bool> _enableSetter;

        public GameplayInputRouter(
            GameplayInput gameplayInput,
            IdleTilemapsInteractionState.Factory idleStateFactory,
            IPauseService pauseService)
        {
            _input = gameplayInput;
            _idleStateFactory = idleStateFactory;
            _pauseService = pauseService;
        }

        public void Initialize()
        {
            ResetState();

            _enableSetter = new(
                SetEnableInternal,
                () => Enabled,
                true);

            _pauseService.PauseChanged += OnPauseChanged;
        }

        public void Tick()
        {
            if (!_input.asset.enabled)
                return;

            State.Update();
        }

        public void Dispose()
        {
            _pauseService.PauseChanged -= OnPauseChanged;
        }

        public void SetEnable(bool enable, int priority, object source)
        {
            _enableSetter.SetValue(enable, priority, source);
        }

        public void ResetEnable(object source)
        {
            _enableSetter.ResetValue(source);
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

        private void OnPauseChanged(bool paused)
        {
            this.SetEnable(!paused, GameplayInputRouterEnablePriority.Pause, this);
        }
    }
}
