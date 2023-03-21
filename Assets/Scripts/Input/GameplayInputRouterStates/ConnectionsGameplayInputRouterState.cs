using ConnectIt.Model;
using UnityEngine;

namespace ConnectIt.Input.GameplayInputRouterStates
{
    public class ConnectionsGameplayInputRouterState : IGameplayInputRouterState
    {
        private readonly GameplayInputRouter _inputRouter;
        private readonly RenderCameraProvider _cameraProvider;
        private readonly Tilemaps _tilemaps;

        public ConnectionsGameplayInputRouterState(
            GameplayInputRouter inputRouter,
            RenderCameraProvider cameraProvider,
            Tilemaps tilemaps)
        {
            _inputRouter = inputRouter;
            _cameraProvider = cameraProvider;
            _tilemaps = tilemaps;
        }

        public void OnInteract(Vector2 positionOnScreen)
        {
            
        }
    }
}
