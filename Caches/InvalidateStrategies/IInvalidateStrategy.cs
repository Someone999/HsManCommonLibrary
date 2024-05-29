namespace HsManCommonLibrary.Caches.InvalidateStrategies;

public interface IInvalidateStrategy<TKey, TValue> where TKey: notnull
{
    public void Evict(MemoryCache<TKey, TValue> cache);
    public bool ShouldEvict(ICacheItem<TValue> items);
}