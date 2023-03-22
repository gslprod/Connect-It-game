using UnityEngine.InputSystem;

namespace ConnectIt.Input.GameplayInputRouterStates
{
    public abstract class GameplayInputRouterState
    {
        public virtual void OnInteractionClick(InputAction.CallbackContext context) { }

        public virtual void OnInteractionPressPerformed(InputAction.CallbackContext context) { }

        public virtual void OnInteractionPressCanceled(InputAction.CallbackContext context) { }

        public virtual void Update() { }

        public virtual void StateExit() { }
    }
}
