namespace HsManCommonLibrary.NestedValues.Attributes;

[AttributeUsage((AttributeTargets.Property))]
public class AutoAssignAttribute : Attribute
{
    public string Path { get; }
    public Type? ConverterType { get; set; }
    public bool IsNestedAssign { get; }

    public AutoAssignAttribute(string? path = null, bool isNestedAssign = false, Type? converterType = null)
    {
        Path = path ?? "";
        IsNestedAssign = isNestedAssign;
        ConverterType = converterType;
    }
}
