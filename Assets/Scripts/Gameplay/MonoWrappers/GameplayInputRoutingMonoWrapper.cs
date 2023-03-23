using ConnectIt.Input;
using UnityEngine;
using Zenject;

namespace ConnectIt.MonoWrappers
{
    public class GameplayInputRoutingMonoWrapper : MonoBehaviour
    {
        private GameplayInputRouter _inputRouter;

        [Inject]
        public void Constructor(GameplayInputRouter inputRouter)
        {
            _inputRouter = inputRouter;
        }

        private void Awake()
        {
            _inputRouter.ResetState();
        }

        private void OnEnable()
        {
            _inputRouter.Enable();
        }

        private void OnDisable()
        {
            _inputRouter.Disable();
        }

        private void Update()
        {
            _inputRouter.Update();
        }
    }
}