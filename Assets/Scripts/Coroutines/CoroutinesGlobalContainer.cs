using ConnectIt.Scenes;
using ConnectIt.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ConnectIt.Coroutines
{
    public class CoroutinesGlobalContainer : MonoBehaviour, ICoroutinesGlobalContainer
    {
        public event Action<Coroutine> CoroutineStopped;

        private readonly List<CoroutineInfo> _coroutines = new();

        private IScenesLoader _scenesLoader;
        private Dictionary<Guid, Coroutine> _localLaunchedCoroutines = new();

        [Inject]
        public void Constructor(
            IScenesLoader scenesLoader)
        {
            _scenesLoader = scenesLoader;
        }

        private void OnEnable()
        {
            _scenesLoader.OnNewSceneAsyncLoadFinished += NewSceneLoadFinished;
        }

        private void OnDisable()
        {
            _scenesLoader.OnNewSceneAsyncLoadFinished -= NewSceneLoadFinished;
        }

        private void NewSceneLoadFinished(SceneType obj)
        {
            for (int i = _coroutines.Count - 1; i >= 0; i--)
            {
                if (!_coroutines[i].Global)
                    StopAndUnregisterByCoroutineInfo(_coroutines[i]);
            }
        }

        public Coroutine StartAndRegisterCoroutine(IEnumerator coroutine, bool global = false)
        {
            Coroutine startedCoroutine = StartCoroutine(coroutine);

            var info = new CoroutineInfo(startedCoroutine, global);

            _coroutines.Add(info);

            return startedCoroutine;
        }

        public void StopAndUnregisterCoroutine(Coroutine coroutine)
        {
            CoroutineInfo coroutineInfo = _coroutines.Find((info) => info.Target == coroutine);
            Assert.IsNotNull(coroutineInfo);

            StopAndUnregisterByCoroutineInfo(coroutineInfo);
        }

        public Coroutine DelayedAction(Action action, YieldInstruction delay = null, bool global = false)
        {
            Assert.ArgIsNotNull(action);

            Guid id = Guid.NewGuid();
            Coroutine coroutine = StartAndRegisterCoroutine(DelayedActionCoroutine(action, delay, id), global);
            _localLaunchedCoroutines.Add(id, coroutine);

            return coroutine;
        }

        private void StopAndUnregisterByCoroutineInfo(CoroutineInfo info)
        {
            _coroutines.Remove(info);

            StopCoroutine(info.Target);
            CoroutineStopped?.Invoke(info.Target);
        }

        private void StopAndUnregisterLocalLaunchedCoroutine(Guid id)
        {
            StopAndUnregisterCoroutine(_localLaunchedCoroutines[id]);

            _localLaunchedCoroutines.Remove(id);
        }

        private IEnumerator DelayedActionCoroutine(Action action, YieldInstruction delay, Guid id)
        {
            yield return delay;

            action();
            StopAndUnregisterLocalLaunchedCoroutine(id);
        }
    }
}