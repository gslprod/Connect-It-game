using System.Collections.Generic;
using System;

namespace ConnectIt.Utilities.Extensions
{
    public static class IEnumerableExtensions
    {
        public static int FindIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            Assert.IsNotNull(items);
            Assert.IsNotNull(predicate);
            
            int retVal = 0;
            foreach (var item in items)
            {
                if (predicate(item))
                    return retVal;

                retVal++;
            }

            return -1;
        }

        public static int IndexOf<T>(this IEnumerable<T> items, T item) { return items.FindIndex(i => EqualityComparer<T>.Default.Equals(item, i)); }
    }
}
