namespace HsManCommonLibrary.Caches;

public interface ICacheItem<out T>
{
    T? Item { get; }
    DateTime? ExpireTime { get; }
    bool IsExpired { get; }
}