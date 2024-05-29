namespace HsManCommonLibrary.Caches.InvalidateStrategies;

public class LastRecentUsageInvalidateStrategy<TKey, TValue> : IInvalidateStrategy<TKey, TValue> where TKey: notnull
{
    public void Evict(MemoryCache<TKey, TValue> cache)
    {
        // 获取所有缓存项的集合
        IEnumerable<KeyValuePair<TKey, ICacheItem<TValue>>> items = cache.GetAllPairs();

        // 遍历所有项，检查是否应该淘汰
        foreach (var kvp in items)
        {
            if (ShouldEvict(kvp.Value))
            {
                // 如果应该淘汰，则从缓存中移除该项
                cache.Remove(kvp.Key);
            }
        }
    }

    public bool ShouldEvict(ICacheItem<TValue> item)
    {
        
        if (item.IsExpired)
        {
            return true;
        }
        
        if (item is not ICacheItemMetadata metadata)
        {
            throw new InvalidOperationException("The cache item does not implement required metadata interface.");
        }
        
        DateTime threshold = DateTime.Now.AddMinutes(-15); // 15分钟内未使用的项将被淘汰
        const int usageThreshold = 10; // 使用计数低于10的项将被淘汰
        
        return metadata.LastUseTime < threshold || metadata.UsedCount < usageThreshold;
    }
}