using System.Collections.Generic;

namespace Core.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string s) => string.IsNullOrEmpty(s);
        public static bool IsNullOrWhiteSpace(this string s) => string.IsNullOrWhiteSpace(s);
        public static string JoinWith(this IEnumerable<string> strings, string separator) => string.Join(separator, strings);
    }
}
