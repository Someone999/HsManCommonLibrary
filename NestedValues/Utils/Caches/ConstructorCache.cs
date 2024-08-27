using System.Reflection;
using HsManCommonLibrary.Reflections;

namespace HsManCommonLibrary.NestedValues.Utils.Caches;

public class ConstructorCache
{
    private DictionaryWrapper<Type, List<ConstructorCacheItem>> _constructorCacheItems =
        DictionaryWrapper<Type, List<ConstructorCacheItem>>.CreateCurrentUsing();

    
    public void CacheConstructor(Type t, MethodFindOptions findOptions)
    {
        var parameterTypes = findOptions.ParameterTypes ?? Array.Empty<Type>();
        if (HasConstructorWithParameterCount(t, parameterTypes.Length ))
        {
            return;
        }

        if (HasConstructorWithTypes(t, parameterTypes))
        {
            return;
        }
        
        TypeWrapper typeWrapper = new TypeWrapper(t);
        var constructor = typeWrapper.GetConstructorFinder().GetConstructor(findOptions);
        AddConstructor(t, parameterTypes, constructor);
    }
    
    private void AddConstructor(Type type, Type?[] parameterTypes, ConstructorInfo? constructorInfo)
    {
        if (!_constructorCacheItems.TryGetValue(type, out var cacheList))
        {
            cacheList = new List<ConstructorCacheItem>();
            _constructorCacheItems.TryAdd(type, cacheList);
        }
        
        var c = new ConstructorCacheItem(parameterTypes, constructorInfo);
        cacheList?.Add(c);
    }
    
    public bool HasConstructorWithTypes(Type type, Type?[] parameterTypes)
    {
        return _constructorCacheItems.TryGetValue(type, out var cacheList) && 
               (cacheList?.Any(c => c.IsSameParameterList(parameterTypes)) ?? false);
    }
    
    public bool HasConstructorWithParameterCount(Type type, int parameterCount)
    {
        return _constructorCacheItems.TryGetValue(type, out var cacheList) &&
               (cacheList?.Any(c => c.IsSameParameterCount(parameterCount)) ?? false);
    }


    public ConstructorInfo? GetConstructor(Type type, Type?[] parameterTypes)
    {
        return !_constructorCacheItems.TryGetValue(type, out var cacheList)
            ? null
            : cacheList?.FirstOrDefault(c => 
                c.IsSameParameterList(parameterTypes))?.ConstructorInfo;
    }

    public static ConstructorCache DefaultInstance { get; } = new ConstructorCache();

}