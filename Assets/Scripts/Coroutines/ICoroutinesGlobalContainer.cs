using System;
using System.Collections;
using UnityEngine;

namespace ConnectIt.Coroutines
{
    public interface ICoroutinesGlobalContainer
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
        void StopCoroutine(Coroutine coroutine);
        Coroutine DelayedAction(Action action, YieldInstruction delay = null);
        Coroutine DelayedAction(Action action, CustomYieldInstruction delay);
        Coroutine DelayedAction(Action action, float delaySec);
        public bool IsAlive();
    }
}