using ConnectIt.Config;
using ConnectIt.Config.Wrappers;
using ConnectIt.Gameplay.Model;
using UnityEngine;
using Zenject;

namespace ConnectIt.Gameplay.LevelLoading
{
    public class CameraSetup : IInitializable
    {
        private readonly RenderCameraProvider _cameraProvider;
        private readonly Tilemaps _tilemaps;
        private readonly GameplayViewConfig _gameplayViewConfig;

        public CameraSetup(
            RenderCameraProvider cameraProvider,
            Tilemaps tilemaps,
            GameplayViewConfig gameplayViewConfig)
        {
            _cameraProvider = cameraProvider;
            _tilemaps = tilemaps;
            _gameplayViewConfig = gameplayViewConfig;
        }

        public void Initialize()
        {
            SetupCameraSizeAndPosition();
        }

        private void SetupCameraSizeAndPosition()
        {
            Bounds mapBounds = _tilemaps.MapLocalBounds;
            Camera camera = _cameraProvider.RenderCamera;

            CameraSettings settings = _gameplayViewConfig.CameraSettings;
            float unclampedSize = mapBounds.size.x + settings.SizePadding;

            camera.orthographicSize = Mathf.Clamp(unclampedSize, settings.MinSize, settings.MaxSize);

            Vector3 cameraPos = mapBounds.center - _tilemaps.CellGap / 2;
            cameraPos.z = camera.transform.position.z;

            camera.transform.position = cameraPos;
        }
    }
}
