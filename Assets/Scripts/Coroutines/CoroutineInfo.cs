using System;
using UnityEngine;

namespace ConnectIt.Coroutines
{
    public class CoroutineInfo
    {
        public Coroutine Target { get; private set; }
        public bool Global { get; private set; }

        public CoroutineInfo(Coroutine target, bool global)
        {
            Target = target;
            Global = global;
        }
    }
}