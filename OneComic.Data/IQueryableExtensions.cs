using OneComic.Core;
using System;
using System.Linq;
using System.Linq.Dynamic;

namespace OneComic.Data
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string order)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (order == null)
                return source;

            var ordering = order
                .Split(',')
                .Select(s => s.StartsWith("-") ? s.Remove(0, 1) + " descending" : s)
                .JoinWith(",");

            if (!ordering.IsNullOrEmpty())
                source = source.OrderBy(ordering);

            return source;
        }
    }
}
