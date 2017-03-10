using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OneComic.Core
{
    public static class DictionaryExtensions
    {
        public static IEnumerable<TKey> GetDiffKeys<TKey, TValue>(
            IReadOnlyDictionary<TKey, TValue> dict1,
            IReadOnlyDictionary<TKey, TValue> dict2)
        {
            var allKeys = dict1.Keys.Concat(dict2.Keys);
            foreach (var key in allKeys)
            {
                if (dict1.ContainsKey(key) &&
                    dict2.ContainsKey(key) &&
                    EqualityComparer<TValue>.Default.Equals(dict1[key], dict2[key]))
                {
                    continue;
                }

                yield return key;
            }
        }
    }
}
