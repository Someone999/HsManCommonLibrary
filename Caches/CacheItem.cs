namespace HsManCommonLibrary.Caches;

internal class CacheItem<T> : ICacheItem<T>, ICacheItemMetadata
{
    public CacheItem(T? item, DateTime? expireTime = null)
    {
        Item = item;
        ExpireTime = expireTime;
    }

    public T? Item { get; set; }
    public DateTime? ExpireTime { get; set; }
    public bool IsExpired => ExpireTime != null && ExpireTime <= DateTime.Now;
    public DateTime LastUseTime { get; set; }
    public int UsedCount { get; set; }
}

public interface ICacheItem<out T>
{
    T? Item { get; }
    DateTime? ExpireTime { get; }
    bool IsExpired { get; }
}

public interface ICacheItemMetadata
{
    public DateTime LastUseTime { get; set; }
    public int UsedCount { get; set; }
}

