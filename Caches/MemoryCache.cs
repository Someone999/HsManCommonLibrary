using HsManCommonLibrary.Caches.InvalidateStrategies;
using HsManCommonLibrary.Exceptions;

namespace HsManCommonLibrary.Caches;

public class MemoryCache<TKey, TValue> where TKey: notnull
{
    private Dictionary<TKey, ICacheItem<TValue>> _cache = new();

    public IInvalidateStrategy<TKey, TValue> DefaultInvalidateStrategy { get; set; } =
        new LastRecentUsageInvalidateStrategy<TKey, TValue>();
    public int MaxCount { get; set; } = 1024;

    public void Add(TKey key, TValue value, DateTime? expireTime = null)
    {
        if (Count >= MaxCount)
        {
            Invalidate(DefaultInvalidateStrategy);
        }
        
        if (Count >= MaxCount)
        {
            throw new CacheFullException();
        }
        
        _cache.Add(key, new CacheItem<TValue>(value, expireTime));
    }

    public KeyValuePair<TKey, ICacheItem<TValue>>[] GetAllPairs() => _cache.ToArray();
    public ICacheItem<TValue>[] GetAllItems() => _cache.Values.ToArray();
    public void Remove(TKey key)
    {
        _cache.Remove(key);
    }

    public void Invalidate(IInvalidateStrategy<TKey, TValue> invalidateStrategy)
    {
        invalidateStrategy.Evict(this);
    }
    
    public TValue? GetValue(TKey key)
    {
        if (!_cache.TryGetValue(key, out var item))
        {
            return default;
        }

        var cacheItem = (CacheItem<TValue>)_cache[key];
        cacheItem.UsedCount++;
        cacheItem.LastUseTime = DateTime.Now;
        return item.Item;
    }
    
    public bool Contains(TKey key)
    {
        return _cache.ContainsKey(key);
    }
    
    public void Clear()
    {
        _cache.Clear();
    }
    
    public int Count => _cache.Count;

    public void RemoveExpiredItems()
    {
        List<TKey> removeKeys = 
            (from cacheItem in _cache where cacheItem.Value.IsExpired select cacheItem.Key)
            .ToList();

        foreach (var key in removeKeys)
        {
            _cache.Remove(key);
        }
    }

    public void Update(TKey key, TValue value, DateTime? expireTime)
    {
        CacheItem<TValue> cacheItem = (CacheItem<TValue>)_cache[key];
        
        cacheItem.Item = value;
        cacheItem.ExpireTime = expireTime;
        
    }
    
    public void UpdateExpireTime(TKey key, DateTime? expireTime)
    {
        CacheItem<TValue> cacheItem = (CacheItem<TValue>)_cache[key];
        cacheItem.ExpireTime = expireTime;
    }
}