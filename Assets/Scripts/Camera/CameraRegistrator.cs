using UnityEngine;
using Zenject;

namespace ConnectIt
{
    public class CameraRegistrator : MonoBehaviour
    {
        private Camera _camera;
        private RenderCameraProvider _renderCameraProvider;

        [Inject]
        public void Constructor(RenderCameraProvider renderCameraProvider)
        {
            _renderCameraProvider = renderCameraProvider;
        }

        private void Awake()
        {
            _camera = GetComponent<Camera>();

            _renderCameraProvider.RegisterCamera(_camera);
        }

        private void OnDestroy()
        {
            _renderCameraProvider.UnregisterCamera(_camera);
        }
    }
}