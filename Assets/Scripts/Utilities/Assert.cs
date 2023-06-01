using System;

namespace ConnectIt.Utilities
{
    public static class Assert
    {
        private const string DefaultMessage = "Validation failed";

        public static void ThatArgIs(bool condition, string message = null)
        {
            if (!condition)
                ThrowArgEx(message);
        }

        public static void ThatArgIs(bool condition1, bool condition2, string message = null)
        {
            ThatArgIs(condition1);
            ThatArgIs(condition2);
        }

        public static void ArgIsNotNull(object arg, string message = null)
        {
            if (arg == null)
                ThrowNullArgEx(message);
        }

        public static void ArgsIsNotNull(object arg1, object arg2, string message = null)
        {
            ArgIsNotNull(arg1, message);
            ArgIsNotNull(arg2, message);
        }

        public static void ArgsIsNotNull(object arg1, object arg2, object arg3, string message = null)
        {
            ArgIsNotNull(arg1, message);
            ArgIsNotNull(arg2, message);
            ArgIsNotNull(arg3, message);
        }

        public static void ArgsIsNotNull(object arg1, object arg2, object arg3, object arg4, string message = null)
        {
            ArgIsNotNull(arg1, message);
            ArgIsNotNull(arg2, message);
            ArgIsNotNull(arg3, message);
            ArgIsNotNull(arg4, message);
        }

        public static void That(bool condition, string message = null)
        {
            if (!condition)
                ThrowInvalidOperationEx(message);
        }

        public static void That(bool condition1, bool condition2, string message = null)
        {
            That(condition1, message);
            That(condition2, message);
        }

        public static void IsNotNull(object arg, string message = null)
        {
            if (arg == null)
                ThrowInvalidOperationEx(message);
        }

        public static void IsNotNull(object arg1, object arg2, string message = null)
        {
            IsNotNull(arg1, message);
            IsNotNull(arg2, message);
        }

        public static void IsNull(object arg, string message = null)
        {
            if (arg != null)
                ThrowInvalidOperationEx(message);
        }

        public static Exception GetFailException(string message = null)
        {
            return CreateInvalidOperationEx(message);
        }

        public static void Fail(string message = null)
        {
            ThrowInvalidOperationEx(message);
        }

        private static void ThrowNullArgEx(string message)
        {
            throw new ArgumentNullException(string.IsNullOrEmpty(message) ? DefaultMessage : message);
        }

        private static void ThrowArgEx(string message)
        {
            throw new ArgumentException(string.IsNullOrEmpty(message) ? DefaultMessage : message);
        }

        private static void ThrowInvalidOperationEx(string message)
        {
            throw new InvalidOperationException(string.IsNullOrEmpty(message) ? DefaultMessage : message);
        }

        private static InvalidOperationException CreateInvalidOperationEx(string message)
        {
            return new InvalidOperationException(string.IsNullOrEmpty(message) ? DefaultMessage : message);
        }
    }
}
