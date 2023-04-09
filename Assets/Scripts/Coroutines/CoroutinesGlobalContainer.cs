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

        private IEnumerator DelayedActionCoroutine(Action action, YieldInstruction delay)
        {
            yield return delay;

            action();
        }
    }
}