namespace Rhynia.Baseline.Util;

public static class DictionaryExtension
{
    /// <summary>
    /// Gets the value associated with the specified key, or adds a default value if the key is not present.
    /// </summary>
    /// <returns>The value associated with the specified key, or a new default value if the key is not present.</returns>
    public static V GetOrAddDefault<K, V>(this Dictionary<K, V> dict, K key)
        where V : new()
    {
        if (!dict.TryGetValue(key, out var value))
        {
            value = new V();
            dict[key] = value;
        }
        return value;
    }

    /// <summary>
    /// Gets the value associated with the specified key, or adds a new value if the key is not present.
    /// </summary>
    /// <returns>The value associated with the specified key, or a new value if the key is not present.</returns>
    public static V GetOrAdd<K, V>(this Dictionary<K, V> dict, K key, Func<V> initializer)
    {
        if (!dict.TryGetValue(key, out var value))
        {
            value = initializer();
            dict[key] = value;
        }
        return value;
    }
}
