using System;

namespace ConnectIt.Utilities
{
    public static class Assert
    {
        private const string DefaultMessage = "Validation failed";

        public static void ThatArgIs(bool condition)
        {
            if (!condition)
                ThrowArgEx();
        }

        public static void ThatArgIs(bool condition1, bool condition2)
        {
            ThatArgIs(condition1);
            ThatArgIs(condition2);
        }

        public static void ArgIsNotNull(object arg)
        {
            if (arg == null)
                ThrowNullArgEx();
        }

        public static void ArgsIsNotNull(object arg1, object arg2)
        {
            ArgIsNotNull(arg1);
            ArgIsNotNull(arg2);
        }

        public static void ArgsIsNotNull(object arg1, object arg2, object arg3)
        {
            ArgIsNotNull(arg1);
            ArgIsNotNull(arg2);
            ArgIsNotNull(arg3);
        }

        public static void ArgsIsNotNull(object arg1, object arg2, object arg3, object arg4)
        {
            ArgIsNotNull(arg1);
            ArgIsNotNull(arg2);
            ArgIsNotNull(arg3);
            ArgIsNotNull(arg4);
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

        public static void IsNotNull(object arg)
        {
            if (arg == null)
                ThrowInvalidOperationEx();
        }

        public static void IsNotNull(object arg1, object arg2)
        {
            IsNotNull(arg1);
            IsNotNull(arg2);
        }

        public static Exception GetFailException()
        {
            return CreateInvalidOperationEx();
        }

        public static void Fail()
        {
            ThrowInvalidOperationEx();
        }

        private static void ThrowNullArgEx()
        {
            throw new ArgumentNullException(DefaultMessage);
        }

        private static void ThrowArgEx()
        {
            throw new ArgumentException(DefaultMessage);
        }

        private static void ThrowInvalidOperationEx()
        {
            throw new InvalidOperationException(DefaultMessage);
        }

        private static InvalidOperationException CreateInvalidOperationEx()
        {
            return new InvalidOperationException(DefaultMessage);
        }
    }
}
