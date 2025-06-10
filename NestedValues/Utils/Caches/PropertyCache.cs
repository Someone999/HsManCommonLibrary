using System.Collections.Concurrent;
using System.Reflection;

namespace HsManCommonLibrary.NestedValues.Utils.Caches;

public class PropertyCache
{
    private DictionaryWrapper<Type, PropertyInfo[]> _propertiesCache =
        DictionaryWrapper<Type, PropertyInfo[]>.CreateCurrentUsing();

    private DictionaryWrapper<PropertyInfo, IPropertyAccessorCache> _accessorCache =
        DictionaryWrapper<PropertyInfo, IPropertyAccessorCache>.CreateCurrentUsing();

    private int _lastVersion = 0,  _version = 0;

    public IPropertyAccessorCache? GetAccessors(PropertyInfo propertyInfo)
    {
        return _accessorCache.Dictionary.TryGetValue(propertyInfo, out var r) ? r : null;
    }

    public PropertyInfo[]? GetProperties(Type t)
    {
        var ret = _propertiesCache.Dictionary.TryGetValue(t, out var r) ? r : null;
        return ret;
    }

    public void AddType(Type t)
    {
        var properties = t.GetProperties();
        if (!_propertiesCache.TryAdd(t, properties))
        {
            return;
        }
        
        _lastVersion = _version;
        Interlocked.Add(ref _version, 1);
    }

    IPropertyAccessorCache AddOrGet(PropertyInfo propertyInfo)
    {
        if (_accessorCache.TryGetValue(propertyInfo, out var accessor) && accessor != null)
        {
            return accessor;
        }
        
        accessor = new ReflectionPropertyAccessorCache(propertyInfo);
        _accessorCache.TryAdd(propertyInfo, accessor);

        return accessor;
    }
    void CacheGetterMethod(PropertyInfo propertyInfo)
    {
        if (propertyInfo.GetMethod == null)
        {
            return;
        }


        AddOrGet(propertyInfo).CacheGetter();
    }

    void CacheSetterMethod(PropertyInfo propertyInfo)
    {
        if (propertyInfo.SetMethod == null)
        {
            return;
        }

        AddOrGet(propertyInfo).CacheSetter();
    }

    void CacheAccessors(PropertyInfo propertyInfo)
    {
        CacheGetterMethod(propertyInfo);
        CacheSetterMethod(propertyInfo);
    }

    public void CacheAllAccessors()
    {
        if (_lastVersion == _version)
        {
            return;
        }
        
        foreach (var propertyInfo in _propertiesCache.Dictionary.SelectMany(pair => pair.Value))
        {
            CacheAccessors(propertyInfo);
        }
    }

    public static PropertyCache DefaultInstance { get; } = new PropertyCache();
}