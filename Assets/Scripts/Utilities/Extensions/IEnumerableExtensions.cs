using System;
using System.Collections.Generic;
using System.Linq;

namespace ConnectIt.Utilities.Extensions
{
    public static class IEnumerableExtensions
    {
        public static int FindIndex<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            Assert.ArgIsNotNull(source);
            Assert.ArgIsNotNull(predicate);

            int count = source.Count();
            for (int i = 0; i < count; i++)
            {
                if (predicate(source.ElementAt(i)))
                    return i;
            }

            return -1;
        }

        public static int FindIndexLast<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            Assert.ArgIsNotNull(source);
            Assert.ArgIsNotNull(predicate);

            int count = source.Count();
            for (int i = count - 1; i >= 0; i--)
            {
                if (predicate(source.ElementAt(i)))
                    return i;
            }

            return -1;
        }

        public static int IndexOf<T>(this IEnumerable<T> items, T item)
            => items.FindIndex(i => EqualityComparer<T>.Default.Equals(item, i));

        public static int LastIndexOf<T>(this IEnumerable<T> items, T item)
            => items.FindIndexLast(i => EqualityComparer<T>.Default.Equals(item, i));
    }
}
