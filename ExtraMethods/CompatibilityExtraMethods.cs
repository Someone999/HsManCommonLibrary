namespace HsManCommonLibrary.ExtraMethods;

public static class CompatibilityExtraMethods
{
#if NETFRAMEWORK || NETCOREAPP2_0 || NETSTANDARD2_0
    public static TValue? GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> d, TKey key)
    {
        return d.GetValueOrDefault(key, default);
    }

    public static TValue? GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> d, TKey key, TValue? defaultVal)
    {
        return d.TryGetValue(key, out var val) ? val : defaultVal;
    }
#endif
}