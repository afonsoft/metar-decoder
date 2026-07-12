#if NETFRAMEWORK || NETSTANDARD2_0
using System.Collections.Generic;

namespace Decoder.Shared
{
    /// <summary>
    /// Polyfill for dictionary lookups on older .NET targets that do not provide
    /// a built-in GetValueOrDefault method.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Gets the value associated with the specified key, or the default value if the key is not present.
        /// </summary>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            return dictionary != null && dictionary.TryGetValue(key, out var value) ? value : defaultValue;
        }
    }
}
#endif
