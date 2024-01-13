using System.Reflection;
using HsManCommonLibrary.Utils;

namespace HsManCommonLibrary.NestedValues.Utils.Caches;

public class ConstructorCacheItem
{
    public ConstructorCacheItem(Type?[] constructorTypes, ConstructorInfo? constructorInfo)
    {
        ConstructorTypes = constructorTypes;
        ConstructorInfo = constructorInfo;
    }
    
    public Type?[] ConstructorTypes { get; }
    public ConstructorInfo? ConstructorInfo { get; }

    public bool IsSameParameterList(Type?[] types)
    {
        return EqualityUtils.SequenceEquals(ConstructorTypes, types);
    }

    public bool IsSameParameterCount(int parameterCount)
    {
        return parameterCount == ConstructorTypes.Length;
    }
}