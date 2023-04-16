using System.Linq;
using UnityEngine.UIElements;

namespace ConnectIt.Utilities.Extensions
{
    public static class IResolvedStyleExtensions
    {
        public static float CalculateMaxTransitionLengthSec(this IResolvedStyle source)
        {
            return source.transitionDelay
                    .Zip(source.transitionDuration,
                        (delay, duration) => delay.GetSeconds() + duration.GetSeconds())
                    .Max();
        }
    }
}
