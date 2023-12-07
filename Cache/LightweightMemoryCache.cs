using System.Data;

namespace HsManCommonLibrary.Cache;

public interface ICacheExpirationPolicy<TKey, TValue> where TKey : notnull
{
    void OnAccess(CacheEntry<TKey, TValue> entry);
    void OnAdd(CacheEntry<TKey, TValue> entry);
    void OnRemove(CacheEntry<TKey, TValue> entry);
    bool HasExpired(CacheEntry<TKey, TValue> cacheEntry, MemoryCacheOption option);
}


public class LessRecentUseCacheExpirationPolicy<TKey, TValue> : ICacheExpirationPolicy<TKey, TValue> where TKey: notnull
{
    private static readonly LinkedList<CacheEntry<TKey, TValue>> UsageList = new LinkedList<CacheEntry<TKey, TValue>>();
    public void OnAccess(CacheEntry<TKey, TValue> entry)
    {
        UsageList.Remove(entry);
        UsageList.AddFirst(entry);
    }

    public void OnAdd(CacheEntry<TKey, TValue> entry)
    {
        UsageList.AddFirst(entry);
    }

    public void OnRemove(CacheEntry<TKey, TValue> entry)
    {
        UsageList.Remove(entry);
    }


    public bool HasExpired(CacheEntry<TKey, TValue> cacheEntry, MemoryCacheOption option)
    {
        var current = UsageList.First;
        if (current == null)
        {
            return false;
        }
        
        for (int i = 0; i < 10; i++)
        {
            var nodeKey = current.Value.Key;
            if (nodeKey == null)
            {
                continue;
            }

            if (nodeKey.Equals(cacheEntry.Key))
            {
                return true;
            }

            current = current.Next;
            if (current == null)
            {
                return false;
            }
        }

        return false;
    }
}

public class CacheEntry<TKey, TValue> where TKey: notnull
{
    public TKey? Key { get; } = default;
    public TValue? Value { get; set; }
}

public class MemoryCacheOption
{
    public long SizeLimit { get; set; }
    public bool AutoScanExpirations { get; set; } = true;
    public TimeSpan ScanInterval { get; set; } = TimeSpan.FromSeconds(10);
}

public class LightweightMemoryCache<TKey, TValue> where TKey: notnull
{
    private readonly Dictionary<TKey, CacheEntry<TKey, TValue>>
        _dictionary = new Dictionary<TKey, CacheEntry<TKey, TValue>>();
    private readonly ReaderWriterLockSlim _readerWriterLock = new ReaderWriterLockSlim();

    public void Add(TKey key, TValue val, ICacheExpirationPolicy<TKey, TValue> expirationPolicy)
    {
        
    }
}

