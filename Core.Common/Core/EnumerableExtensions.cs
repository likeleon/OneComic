using System;
using System.Collections.Generic;

namespace Core.Common.Core
{
    public static class EnumerableExtensions
    {
        public static void Do<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var e in enumerable)
                action(e);
        }
    }
}
