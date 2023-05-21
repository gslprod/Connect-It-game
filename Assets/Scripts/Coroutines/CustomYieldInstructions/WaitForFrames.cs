using ConnectIt.Utilities;
using UnityEngine;

namespace ConnectIt.Coroutines.CustomYieldInstructions
{
    public class WaitForFrames : CustomYieldInstruction
    {
        public override bool keepWaiting => _elapsedFrames++ < _framesCountToWait;

        private long _framesCountToWait;
        private long _elapsedFrames;

        public WaitForFrames(long framesCount)
        {
            Assert.ThatArgIs(framesCount >= 0);

            _framesCountToWait = framesCount;
        }
    }
}
