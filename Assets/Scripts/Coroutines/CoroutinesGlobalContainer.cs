using ConnectIt.Utilities;
using System;
using System.Collections;
using UnityEngine;

namespace ConnectIt.Coroutines
{
    public class CoroutinesGlobalContainer : MonoBehaviour, ICoroutinesGlobalContainer
    {
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