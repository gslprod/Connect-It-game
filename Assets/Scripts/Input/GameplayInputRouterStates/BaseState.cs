using UnityEngine.InputSystem;

namespace ConnectIt.Input.GameplayInputRouterStates
{
    public abstract class BaseState
    {
        protected readonly GameplayInput input;
        protected readonly GameplayInputRouter inputRouter;

        public BaseState(GameplayInput input,
            GameplayInputRouter inputRouter)
        {
            this.input = input;
            this.inputRouter = inputRouter;
        }

        public virtual void OnInteractionClick(InputAction.CallbackContext context) { }

        public virtual void OnInteractionPressCanceled(InputAction.CallbackContext context) { }

        public virtual void OnInteractionPressPerformed(InputAction.CallbackContext context) { }

        public virtual void StateExit() { }

        public virtual void Update() { }
    }
}