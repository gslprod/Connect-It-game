using ConnectIt.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectIt
{
    public class RenderCameraProvider
    {
        public Camera RenderCamera {  get; private set; }

        private List<Camera> _camerasList = new();

        public void RegisterCamera(Camera camera)
        {
            Assert.That(!_camerasList.Contains(camera));

            _camerasList.Add(camera);

            if (RenderCamera == null)
                SetRenderCamera(camera);
        }

        public void UnregisterCamera(Camera camera)
        {
            Assert.ArgIsNotNull(camera);
            Assert.That(_camerasList.Remove(camera));

            if (RenderCamera == camera)
            {
                RenderCamera = null;

                if (_camerasList.Count != 0)
                    SetRenderCamera(_camerasList[0]);
            }
        }

        public void SetRenderCamera(Camera toSet)
        {
            Assert.ArgIsNotNull(toSet);
            Assert.That(_camerasList.Contains(toSet));

            foreach (var camera in _camerasList)
            {
                if (camera != toSet)
                    camera.enabled = false;
                else
                    camera.enabled = true;
            }

            RenderCamera = toSet;
        }
    }
}
