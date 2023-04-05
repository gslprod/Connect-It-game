using System;
using System.Collections;
using UnityEngine;

namespace ConnectIt.Coroutines
{
    public interface ICoroutinesGlobalContainer
    {
        event Action<Coroutine> CoroutineStopped;

        Coroutine StartAndRegisterCoroutine(IEnumerator coroutine, bool global = false);
        void StopAndUnregisterCoroutine(Coroutine coroutine);
    }
}