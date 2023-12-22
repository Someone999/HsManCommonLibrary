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
    
}