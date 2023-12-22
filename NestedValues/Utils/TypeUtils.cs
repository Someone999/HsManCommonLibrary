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

        var isDictionary = nestedValueStore.GetValue() is Dictionary<string, INestedValueStore>;
        var isCompatible =
            HsManCommonLibrary.Utils.TypeUtils.IsCompatibleType(nestedValueStore.GetValue(), elementType);
        
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

            var typeWrapper = new TypeWrapper(t);
            var cons = typeWrapper.GetConstructor(new MethodFindOptions()
            {
                ParameterTypes = parameters?.Select(p => p?.GetType()).ToArray() ?? Array.Empty<Type>()
            });

            if (cons == null)
            {
                ins = typeWrapper.CreateInstance(null);
                return ins != null;
            }
            
            ins = typeWrapper.CreateInstance(parameters);
            return ins != null;
        }
        catch (Exception e)
        {
            ins = null;
            return false;
        }
       

    }
}