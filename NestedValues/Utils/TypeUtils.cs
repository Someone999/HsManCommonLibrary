using HsManCommonLibrary.Exceptions;
using HsManCommonLibrary.NestedValues.Utils.Caches;
using HsManCommonLibrary.Reflections;

namespace HsManCommonLibrary.NestedValues.Utils;

public static class TypeUtils
{
    public static bool NeedToCreateNewObject(Type elementType, INestedValueStore nestedValueStore)
    {
        if (nestedValueStore.GetValue() == null)
        {
            return elementType.IsValueType;
        }

        var val0 = nestedValueStore.GetValue();
        bool isCompatible = TypeCompareCache.DefaultInstance.GetIsCompatible(val0, elementType);
        
        if (isCompatible)
        {
            return false;
        }

        var isDictionary = nestedValueStore.GetValue() is Dictionary<string, INestedValueStore>;
        
        
        return !isCompatible && isDictionary;
    }
    

    
    
    public static bool TryCreateInstance(Type t, INestedValueStore nestedValueStore, AssignOptions? assignOptions,
        out object? ins)
    {
        try
        {
            object?[]? parameters =
                (nestedValueStore.GetValue() as Dictionary<string, INestedValueStore>)?
                .Values.Select(v => v.GetValue()).ToArray();

            if (parameters == null)
            {
                assignOptions?.ConstructorParameters.TryGetValue(t, out parameters);
            }

            var types = parameters?.Select(p => p?.GetType()).ToArray() ?? Array.Empty<Type>();
            var cachedCons = ConstructorCache.DefaultInstance.GetConstructor(t, types);
            if (cachedCons == null)
            {
                cachedCons = ConstructorCache.DefaultInstance.GetConstructor(t, Type.EmptyTypes);
            }
            else
            {
                ins = cachedCons.Invoke(parameters);
                return true;
            }

            if (cachedCons != null)
            {
                ins = cachedCons.Invoke(null);
                return true;
            }

            var parameterizedMethodFindOptions = new MethodFindOptions()
            {
                ParameterTypes = types
            };
            var typeWrapper = new TypeWrapper(t);
            var cons = typeWrapper.GetConstructor(parameterizedMethodFindOptions);
            
            if (cons == null)
            {
                ConstructorCache.DefaultInstance.CacheConstructor(t, MethodFindOptions.Empty);
                cons = ConstructorCache.DefaultInstance.GetConstructor(t, Type.EmptyTypes) ?? 
                       throw new HsManInternalException();
                
                ins = cons.Invoke(null);
                return true;
            }
            
            ConstructorCache.DefaultInstance.CacheConstructor(t, MethodFindOptions.Empty);
            ins = typeWrapper.CreateInstance(parameters);
            return ins != null;
        }
        catch (Exception)
        {
            ins = null;
            return false;
        }
    }
}