using HsManCommonLibrary.BindableValues;

namespace HsManCommonLibrary.Utils;

public static class BindableValueUtils
{
    public static bool IsSameType<TCheckType>(IBindableValue bindableValue)
    {
        if (bindableValue.Value == null && bindableValue.DefaultValue == null)
        {
            return false;
        }

        var checkVal = bindableValue.Value ?? bindableValue.DefaultValue;
        if (checkVal == null)
        {
            return false;
        }
        
        return checkVal.GetType() == typeof(TCheckType);
    }

    public static bool IsCompatibleType<TCheckType>(IBindableValue bindableValue)
    {
        if (bindableValue.Value == null && bindableValue.DefaultValue == null)
        {
            return false;
        }

        var checkVal = bindableValue.Value ?? bindableValue.DefaultValue;
        
        return checkVal is TCheckType;
    }
    
    public static bool IsSameType<TCheckType, TValueType>(IBindableValue<TValueType> bindableValue)
    {
        if (bindableValue.Value == null && bindableValue.DefaultValue == null)
        {
            return false;
        }

        return typeof(TValueType) == typeof(TCheckType);
    }

    public static bool IsCompatibleType<TCheckType, TValueType>(IBindableValue<TValueType> bindableValue)
    {
        return typeof(TCheckType).IsAssignableFrom(typeof(TValueType));
    }
}