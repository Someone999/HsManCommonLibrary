using System.Reflection;

namespace HsManCommonLibrary.NestedValues.Utils.Assigners;

public interface IObjectAssigner
{
    void Assign(PropertyInfo propertyInfo, object? ins, object? val, AssignOptions? assignOptions);
}