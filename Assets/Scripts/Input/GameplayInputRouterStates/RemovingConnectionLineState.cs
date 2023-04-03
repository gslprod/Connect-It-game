using ConnectIt.Config;
using ConnectIt.Gameplay.Model;
using ConnectIt.Gameplay.Time;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace ConnectIt.Input.GameplayInputRouterStates
{
    public class RemovingConnectionLineState : BaseTilemapsInteractionState
    {
        private readonly GameplayLogicConfig _gameplayConfig;
        private readonly ConnectionLine _removingLine;
        private readonly ITimeProvider _timeProvider;

        private float _expiredTimeSec;

        public RemovingConnectionLineState(
            GameplayInput input,
            GameplayInputRouter inputRouter,
            RenderCameraProvider cameraProvider,
            Tilemaps tilemaps,
            GameplayLogicConfig gameplayConfig,
            ITimeProvider timeProvider,
            ConnectionLine removingLine) : base(input, inputRouter, cameraProvider, tilemaps)
        {
            _gameplayConfig = gameplayConfig;
            _removingLine = removingLine;
            _timeProvider = timeProvider;
        }

        public override void OnInteractionPressCanceled(InputAction.CallbackContext context)
        {
            inputRouter.ResetState();
        }

        public override void Update()
        {
            _expiredTimeSec += _timeProvider.DeltaTime;

            if (_expiredTimeSec >= _gameplayConfig.RemoveConnectionLineHoldDurationSec)
            {
                _removingLine.Dispose();

                inputRouter.ResetState();
            }
        }

        public class Factory : PlaceholderFactory<ConnectionLine, RemovingConnectionLineState> { }
    }
}
