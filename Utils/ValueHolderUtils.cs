using HsManCommonLibrary.ValueHolders;

namespace HsManCommonLibrary.Utils;

public static class ValueHolderUtils
{
    public static bool IsSameType<TCheckType>(IValueHolder valueHolder)
    {
        if (valueHolder.Value == null && valueHolder.DefaultValue == null)
        {
            return false;
        }

        var checkVal = valueHolder.Value ?? valueHolder.DefaultValue;
        if (checkVal == null)
        {
            return false;
        }
        
        return checkVal.GetType() == typeof(TCheckType);
    }

    public static bool IsCompatibleType<TCheckType>(IValueHolder valueHolder)
    {
        if (valueHolder.Value == null && valueHolder.DefaultValue == null)
        {
            return false;
        }

        var checkVal = valueHolder.Value ?? valueHolder.DefaultValue;
        
        return checkVal is TCheckType;
    }
    
    public static bool IsSameType<TCheckType, TValueType>(IValueHolder<TValueType> valueHolder)
    {
        if (valueHolder.Value == null && valueHolder.DefaultValue == null)
        {
            return false;
        }

        return typeof(TValueType) == typeof(TCheckType);
    }

    public static bool IsCompatibleType<TCheckType, TValueType>(IValueHolder<TValueType> valueHolder)
    {
        return typeof(TCheckType).IsAssignableFrom(typeof(TValueType));
    }
}