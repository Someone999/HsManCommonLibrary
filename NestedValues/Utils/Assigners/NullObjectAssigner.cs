using System.Reflection;
using HsManCommonLibrary.NestedValues.Utils.Caches;

namespace HsManCommonLibrary.NestedValues.Utils.Assigners;

public class NullObjectAssigner : IObjectAssigner
{
    public void Assign(PropertyInfo propertyInfo, object? ins, object? val, AssignOptions? assignOptions)
    {
        if (val != null)
        {
            return;
        }
        
        propertyInfo.SetValue(ins, val);
    }
    
    public static IObjectAssigner ObjectAssigner { get; } = new NullObjectAssigner();
}