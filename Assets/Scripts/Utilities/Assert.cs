using System;

namespace ConnectIt.Utilities
{
    public static class Assert
    {
        private const string DefaultMessage = "Validation failed";

        public static void IsNotNull(object obj)
        {
            if (obj == null)
                ThrowNullArgEx();
        }

        public static void That(bool condition)
        {
            if (!condition)
                ThrowInvalidOperationEx();
        }

        private static void ThrowNullArgEx()
        {
            throw new ArgumentNullException(DefaultMessage);
        }

        private static void ThrowInvalidOperationEx()
        {
            throw new InvalidOperationException(DefaultMessage);
        }
    }
}
