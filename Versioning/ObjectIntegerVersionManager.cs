using HsManCommonLibrary.Utils;
using HsManCommonLibrary.ValueWatchers;

namespace HsManCommonLibrary.Versioning;

public class ObjectIntegerVersionManager<T> : IObjectVersionManager<T, int>
{
    public ObjectIntegerVersionManager(T? obj = default, int version = 0)
    {
        CurrentObject = obj;
        Version = version;
    }
    public T? CurrentObject { get; private set; }
    public IEqualityComparer<T>? EqualityComparer { get; set; }

    public IVersionIncrementStrategy<int> VersionIncrementStrategy { get; set; } =
        new IntegerVersionIncrementStrategy();

    private bool ObjectEqualsInternal(T? obj)
    {
        if (ReferenceEquals(obj, CurrentObject))
        {
            return true;
        }
        
        if (CurrentObject == null || obj == null)
        {
            return EqualityUtils.Equals(CurrentObject, obj);
        }

        return EqualityComparer?.Equals(CurrentObject, obj) ?? EqualityUtils.Equals(CurrentObject, obj);
    }
    
    public bool Update(T? obj)
    {
        bool isSameVal = ObjectEqualsInternal(obj);
        if (isSameVal)
        {
            return false;
        }
        
        var objChange = new ValueChangedEventData<T>(CurrentObject, obj);
        int nextVersion = VersionIncrementStrategy.IncrementVersion(Version);
        var versionChange = new ValueChangedEventData<int>(Version, nextVersion);
        CurrentObject = obj;
        Version = nextVersion;

        VersionChanged?.Invoke(objChange, versionChange);
        return true;
    }

    public void Reset(int initialVersion)
    {
        var objChange = new ValueChangedEventData<T>(CurrentObject, default);
        var versionChange = new ValueChangedEventData<int>(Version, initialVersion);
        CurrentObject = default;
        Version = initialVersion;
        VersionChanged?.Invoke(objChange, versionChange);
    }

    public void SetObjectAndVersion(T? obj, int version)
    {
        var objChange = new ValueChangedEventData<T>(CurrentObject, obj);
        var versionChange = new ValueChangedEventData<int>(Version, version);
        CurrentObject = obj;
        Version = version;
        VersionChanged?.Invoke(objChange, versionChange);
    }

    public int Version { get; private set; }

    public event VersionChangedEventHandler<T, int>? VersionChanged;
}