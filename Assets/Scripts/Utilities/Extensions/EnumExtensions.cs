using System;

namespace ConnectIt.Utilities.Extensions
{
    public static class EnumExtensions
    {
        public static int ToInt(this Enum source)
            => Convert.ToInt32(source);
    }
}
