using System;
using System.Collections;
using UnityEngine;

namespace ConnectIt.Coroutines
{
    public interface ICoroutinesGlobalContainer
    {
        event Action<Coroutine> CoroutineStopped;

        Coroutine DelayedAction(Action action, YieldInstruction delay = null, bool global = false);
        Coroutine StartAndRegisterCoroutine(IEnumerator coroutine, bool global = false);
        void StopAndUnregisterCoroutine(Coroutine coroutine);
    }
}