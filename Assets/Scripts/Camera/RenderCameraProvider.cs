using ConnectIt.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectIt
{
    public class RenderCameraProvider
    {
        public Camera Current {  get; private set; }

        private List<Camera> _camerasList = new();

        public void RegisterCamera(Camera camera)
        {
            _camerasList.Add(camera);
            Assert.That(!_camerasList.Contains(camera));

            _camerasList.Add(camera);

            if (Current == null)
                SetRenderCamera(camera);
        }

        public void UnregisterCamera(Camera camera)
        {
            Assert.IsNotNull(camera);
            Assert.That(_camerasList.Remove(camera));

            if (Current == camera)
            {
                Current = null;

                if (_camerasList.Count != 0)
                    SetRenderCamera(_camerasList[0]);
            }
        }

        public void SetRenderCamera(Camera toSet)
        {
            Assert.IsNotNull(toSet);
            Assert.That(_camerasList.Contains(toSet));

            foreach (var camera in _camerasList)
            {
                if (camera != toSet)
                    camera.enabled = false;
                else
                    camera.enabled = true;
            }

            Current = toSet;
        }
    }
}
