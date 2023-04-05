using ConnectIt.Coroutines;
using ConnectIt.Utilities;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ConnectIt.Scenes
{
    public class ScenesLoader : IScenesLoader
    {
        public event Action<SceneType> OnNewSceneAsyncLoadStarted;
        public event Action<SceneType> OnNewSceneAsyncLoadFinished;
        public event Action<SceneType, float> OnNewSceneAsyncLoading;

        private readonly CoroutinesGlobalContainer _coroutinesGlobalContainer;

        private Coroutine _currentSceneLoadingCoroutine;

        public ScenesLoader(
            CoroutinesGlobalContainer coroutinesGlobalContainer)
        {
            _coroutinesGlobalContainer = coroutinesGlobalContainer;
        }

        public bool TryGoToSceneAsync(SceneType sceneType)
        {
            if (_currentSceneLoadingCoroutine != null)
                return false;

            _currentSceneLoadingCoroutine = _coroutinesGlobalContainer.StartAndRegisterCoroutine(AsyncLoadingScene(sceneType));
            _coroutinesGlobalContainer.CoroutineStopped += OnCoroutineStopped;

            return true;
        }

        private void OnCoroutineStopped(Coroutine coroutine)
        {
            if (coroutine != _currentSceneLoadingCoroutine)
                return;

            _currentSceneLoadingCoroutine = null;

            _coroutinesGlobalContainer.CoroutineStopped -= OnCoroutineStopped;
        }

        private IEnumerator AsyncLoadingScene(SceneType sceneType)
        {
            var asyncLoad = SceneManager.LoadSceneAsync((int)sceneType, new LoadSceneParameters(LoadSceneMode.Single, LocalPhysicsMode.None));
            if (asyncLoad == null)
                Assert.Fail();

            OnNewSceneAsyncLoadStarted?.Invoke(sceneType);
            while (!asyncLoad.isDone)
            {
                OnNewSceneAsyncLoading?.Invoke(sceneType, asyncLoad.progress);
                yield return null;
            }

            OnNewSceneAsyncLoadFinished?.Invoke(sceneType);

            _coroutinesGlobalContainer.StopAndUnregisterCoroutine(_currentSceneLoadingCoroutine);
        }
    }
}
