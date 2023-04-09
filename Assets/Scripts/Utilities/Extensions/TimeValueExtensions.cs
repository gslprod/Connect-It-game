using UnityEngine.UIElements;

namespace ConnectIt.Utilities.Extensions
{
    public static class TimeValueExtensions
    {
        public static float GetSeconds(this TimeValue source)
        {
            return source.unit switch
            {
                TimeUnit.Second => source.value,
                TimeUnit.Millisecond => source.value / 1000,

                _ => throw Assert.GetFailException(),
            };
        }
    }
}
