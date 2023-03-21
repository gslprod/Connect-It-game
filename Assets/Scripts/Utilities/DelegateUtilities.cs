using System;

namespace ConnectIt.Utilities
{
    public static class DelegateUtilities
    {
        public enum InvokeOrder
        {
            Normal,
            Reversed
        }

        public static object[] InvokeAllAndGetResults(this Delegate source, object[][] args = null, InvokeOrder order = InvokeOrder.Normal)
        {
            Delegate[] invocationMembers = source.GetInvocationList();
            var results = new object[invocationMembers.Length];
            args ??= new object[invocationMembers.Length][];

            int lastElementIndex = invocationMembers.Length - 1;
            for (int i = 0; i <= lastElementIndex; i++)
            {
                int index = order switch
                {
                    InvokeOrder.Normal => i,
                    InvokeOrder.Reversed => lastElementIndex - i,
                    _ => -1
                };

                if (index < 0)
                    Assert.Fail();

                results[index] = invocationMembers[index].DynamicInvoke(args[index]);
            }

            return results;
        }
    }
}
