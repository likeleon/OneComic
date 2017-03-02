using System;
using System.Collections;
using System.Collections.Generic;

namespace OneComic.Core
{
    public sealed class Cache<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _cache;
        private readonly Func<TKey, TValue> _loader;

        public Cache(Func<TKey, TValue> loader)
            : this(loader, EqualityComparer<TKey>.Default)
        {
        }

        public Cache(Func<TKey, TValue> loader, IEqualityComparer<TKey> comparer)
        {
            if (loader == null)
                throw new ArgumentNullException(nameof(loader));

            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            _loader = loader;
            _cache = new Dictionary<TKey, TValue>(comparer);
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                if (!_cache.TryGetValue(key, out value))
                    _cache.Add(key, value = _loader(key));
                return value;
            }
        }

        public int Count => _cache.Count;
        public IEnumerable<TKey> Keys => _cache.Keys;
        public IEnumerable<TValue> Values => _cache.Values;

        public bool ContainsKey(TKey key) => _cache.ContainsKey(key);
        public bool TryGetValue(TKey key, out TValue value) => _cache.TryGetValue(key, out value);
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _cache.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
