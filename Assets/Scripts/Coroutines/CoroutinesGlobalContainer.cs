using ConnectIt.Utilities;
using System;
using System.Collections;
using UnityEngine;

namespace ConnectIt.Coroutines
{
    public class CoroutinesGlobalContainer : MonoBehaviour, ICoroutinesGlobalContainer
    {
        private void Awake()
        {
            //todo
            //for some reason on android Zenject doesnt marks this object as DontDestroyOnLoad, whether this object is creating from ProjectContext
            //in the editor playmode this problem doesnt appear
            DontDestroyOnLoad(gameObject);
        }

        public Coroutine DelayedAction(Action action, YieldInstruction delay = null)
        {
            Assert.ArgIsNotNull(action);

            return StartCoroutine(DelayedActionCoroutine(action, delay));
        }

        public Coroutine DelayedAction(Action action, CustomYieldInstruction delay)
        {
            Assert.ArgIsNotNull(action);

            return StartCoroutine(DelayedActionCoroutine(action, delay));
        }

        public Coroutine DelayedAction(Action action, float delaySec)
        {
            return DelayedAction(action, new WaitForSeconds(delaySec));
        }

        private IEnumerator DelayedActionCoroutine(Action action, YieldInstruction delay)
        {
            yield return delay;

            action();
        }

        private IEnumerator DelayedActionCoroutine(Action action, CustomYieldInstruction delay)
        {
            yield return delay;

            action();
        }
    }
}