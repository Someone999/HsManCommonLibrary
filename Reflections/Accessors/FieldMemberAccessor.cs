using System.Reflection;

namespace HsManCommonLibrary.Reflections.Accessors;

public class FieldMemberAccessor : IMemberAccessor
{
    private readonly FieldInfo _fieldInfo;

    public FieldMemberAccessor(FieldInfo fieldInfo)
    {
        _fieldInfo = fieldInfo;
    }
    public object? GetValue(object? instance, params object?[]? args) => _fieldInfo.GetValue(instance);
    public void SetValue(object? instance, object? value, params object?[]? args) => _fieldInfo.SetValue(instance, value);

}