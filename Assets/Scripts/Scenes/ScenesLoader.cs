﻿using ConnectIt.Coroutines;
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

        public SceneType ActiveScene => (SceneType)SceneManager.GetActiveScene().buildIndex;

        public bool SceneLoading => _currentSceneLoadingCoroutine != null;

        private readonly ICoroutinesGlobalContainer _coroutinesGlobalContainer;

        private Coroutine _currentSceneLoadingCoroutine;

        public ScenesLoader(
            ICoroutinesGlobalContainer coroutinesGlobalContainer)
        {
            _coroutinesGlobalContainer = coroutinesGlobalContainer;
        }

        public bool TryGoToSceneAsync(SceneType sceneType)
        {
            if (SceneLoading)
                return false;

            _currentSceneLoadingCoroutine = _coroutinesGlobalContainer.StartCoroutine(AsyncLoadingScene(sceneType));

            return true;
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

            _currentSceneLoadingCoroutine = null;
        }
    }
}
