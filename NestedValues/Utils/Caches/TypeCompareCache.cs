using System.Collections.Concurrent;

namespace HsManCommonLibrary.NestedValues.Utils.Caches;

public class TypeCompareCache
{
    private DictionaryWrapper<Type, ComparisionResult> _cache =
        DictionaryWrapper<Type, ComparisionResult>.CreateCurrentUsing();

    public void CacheResult(Type srcType, Type destType, bool isCompatible)
    {
        _cache.TryAdd(srcType, new ComparisionResult());
        _cache[srcType].CacheResult(destType, isCompatible);
    }

    public bool GetIsCompatible(object? srcObj, Type destType)
    {
        return srcObj == null
            ? HsManCommonLibrary.Utils.TypeUtils.IsCompatibleType(srcObj, destType)
            : GetIsCompatible(srcObj.GetType(), destType);
    }

    public bool GetIsCompatible(Type srcType, Type destType)
    {
        /*if (_cache.TryGetValue(srcType, out var result))
        {
            return result.IsCompatible(destType);
        }*/
        
        var compatible = HsManCommonLibrary.Utils.TypeUtils.IsCompatibleType(srcType, destType);
        /*var comparisionResult = new ComparisionResult();
        comparisionResult.CacheResult(destType, compatible);
        _cache.TryAdd(srcType, comparisionResult);*/
        return compatible;
    }

    public static TypeCompareCache DefaultInstance { get; } = new TypeCompareCache();
}