using HsManCommonLibrary.NestedValues.NestedValueConverters;

namespace HsManCommonLibrary.NestedValues.Utils;

public class ConvertOptions
{
    public INestedValueStoreConverter? Converter { get; set; }
    public Type TargetType { get; set; } = typeof(object);
    public bool AllowAutoAdapt { get; set; } = true;
}