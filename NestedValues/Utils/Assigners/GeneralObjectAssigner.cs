using System.Reflection;
using HsManCommonLibrary.NestedValues.Utils.Caches;

namespace HsManCommonLibrary.NestedValues.Utils.Assigners;

public class GeneralObjectAssigner : IObjectAssigner
{
    public void Assign(PropertyInfo propertyInfo, object? ins, object? val, AssignOptions? assignOptions)
    {
        if (val == null)
        {
            throw new ArgumentNullException(nameof(val));
        }
        
        var propertySetter = PropertyCache.DefaultInstance.GetAccessors(propertyInfo);
        if (propertySetter == null)
        {
            return;
        }
        
        if (TypeCompareCache.DefaultInstance.GetIsCompatible(val, propertyInfo.PropertyType))
        {
            propertySetter.SetValue(ins, val);
        }

        Type propertyType = propertyInfo.PropertyType;
        var nestedVal = (INestedValueStore)val;
        var currentVal = nestedVal.GetValue();
        if (NullNestedValue.Value.Equals(currentVal))
        {
            currentVal = null;
            propertySetter.SetValue(ins, currentVal);
            return;
        }

        if (TypeUtils.NeedToCreateNewObject(propertyType, nestedVal))
        {
            TypeUtils.TryCreateInstance(propertyType, nestedVal, assignOptions, out var obj);
            propertySetter.SetValue(ins, obj);
            return;
        }

        if (TypeCompareCache.DefaultInstance.GetIsCompatible(currentVal, propertyType))
        {
            propertySetter.SetValue(ins, currentVal);
            return;
        }

        if (assignOptions?.ConvertOptions.Converter != null)
        {
            currentVal = assignOptions.ConvertOptions.Converter.Convert(nestedVal);
            propertySetter.SetValue(ins, currentVal);
            return;
        }

        var allowAutoAdapt = assignOptions?.ConvertOptions.AllowAutoAdapt ?? true;
        if (allowAutoAdapt && currentVal is IConvertible)
        {
            if (propertyType.IsEnum)
            {
                propertyType = Enum.GetUnderlyingType(propertyType);
            }

            currentVal = Convert.ChangeType(currentVal, propertyType);
        }
        else
        {
            throw new InvalidOperationException("Auto-adapt is disallowed");
        }

        propertySetter.SetValue(ins, currentVal);
    }
    
    public static IObjectAssigner ObjectAssigner { get; } = new GeneralObjectAssigner();
}