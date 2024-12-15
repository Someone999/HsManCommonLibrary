namespace HsManCommonLibrary.Versioning;

public interface IObjectVersionManager<T, TVersion>
{
    T? CurrentObject { get; }
    IEqualityComparer<T>? EqualityComparer { get; set; }
    IVersionIncrementStrategy<TVersion> VersionIncrementStrategy { get; set; }
    bool Update(T? obj);
    void Reset(TVersion initialVersion);
    void SetObjectAndVersion(T? obj, TVersion version);
    TVersion Version { get; }
    event VersionChangedEventHandler<T, TVersion>? VersionChanged;
}