using UnityEngine;

namespace ConnectIt.Input.GameplayInputRouterStates
{
    public interface IGameplayInputRouterState
    {
        public void OnInteract(Vector2 positionOnScreen);
    }
}
