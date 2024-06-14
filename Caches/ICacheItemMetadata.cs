namespace HsManCommonLibrary.Caches;

public interface ICacheItemMetadata
{
    public DateTime LastUseTime { get; set; }
    public int UsedCount { get; set; }
}