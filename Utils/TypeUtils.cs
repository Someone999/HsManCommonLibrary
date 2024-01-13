using HsManCommonLibrary.NestedValues.Utils;

namespace HsManCommonLibrary.Utils;

public static class TypeUtils
{
    public static bool IsCompatibleType(object? sourceObj, Type targetType)
    {
        if (sourceObj == null)
        {
            return targetType.IsInterface || targetType.IsClass || Nullable.GetUnderlyingType(targetType) != null;
        }

        return targetType.IsInstanceOfType(sourceObj);
    }

    
    public static bool IsCompatibleType(Type sourceObj, Type targetType)
    {
        return targetType.IsInstanceOfType(sourceObj);
    }
}