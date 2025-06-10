namespace HsManCommonLibrary.NestedValues.Utils.Caches;

public interface IPropertyAccessorCache
{
    object? GetValue(object? instance, params object?[]? parameters);
    void SetValue(object? instance, params object?[]? parameters);
    void CacheGetter();
    void CacheSetter();
    void CacheAll();
}