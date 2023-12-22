using HsManCommonLibrary.NestedValues.NestedValueConverters;

namespace HsManCommonLibrary.NestedValues.Utils;

public class AssignOptions
{
    public Dictionary<Type, object?[]> ConstructorParameters { get; set; } = new Dictionary<Type, object?[]>();
    public ConvertOptions ConvertOptions { get; } = new ConvertOptions();
}