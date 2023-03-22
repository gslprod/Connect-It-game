using ConnectIt.Config;
using ConnectIt.Model;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ConnectIt.Input.GameplayInputRouterStates
{
    public class RemovingConnectionLineState : BaseTilemapsInteractionState
    {
        private readonly GameplayConfig _gameplayConfig;
        private readonly ConnectionLine _removingLine;

        private float _expiredTimeSec;

        public RemovingConnectionLineState(
            GameplayInput input,
            GameplayInputRouter inputRouter,
            RenderCameraProvider cameraProvider,
            Tilemaps tilemaps,
            GameplayConfig gameplayConfig,
            ConnectionLine removingLine) : base(input, inputRouter, cameraProvider, tilemaps)
        {
            _gameplayConfig = gameplayConfig;
            _removingLine = removingLine;
        }

        public override void OnInteractionPressCanceled(InputAction.CallbackContext context)
        {
            inputRouter.ResetState();
        }

        public override void Update()
        {
            _expiredTimeSec += Time.deltaTime;

            if (_expiredTimeSec >= _gameplayConfig.RemoveConnectionLineHoldDurationSec)
            {
                _removingLine.Remove();

                inputRouter.ResetState();
            }
        }
    }
}
