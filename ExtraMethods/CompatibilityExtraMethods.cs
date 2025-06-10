namespace HsManCommonLibrary.ExtraMethods;

public static class CompatibilityExtraMethods
{
#if NETFRAMEWORK || !NETCOREAPP2_1_OR_GREATER
    public static TValue? GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> d, TKey key)
    {
        return d.GetValueOrDefault(key, default(TValue));
    }

    public static TValue? GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> d, TKey key, TValue? defaultVal)
    {
        return d.TryGetValue(key, out var val) ? val : defaultVal;
    }
#endif
}