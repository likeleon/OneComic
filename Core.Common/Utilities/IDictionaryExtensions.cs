﻿using System.Collections.Generic;

namespace Core.Common.Utilities
{
    public static class IDictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue value;
            if (dictionary.TryGetValue(key, out value))
                return value;

            return default(TValue);
        }
    }
}
