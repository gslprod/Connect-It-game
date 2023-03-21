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

        public static void IsNotNull(object obj1, object obj2)
        {
            IsNotNull(obj1);
            IsNotNull(obj2);
        }

        public static void That(bool condition)
        {
            if (!condition)
                ThrowInvalidOperationEx();
        }

        public static void That(bool condition1, bool condition2)
        {
            That(condition1);
            That(condition2);
        }

        public static void Fail()
        {
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
