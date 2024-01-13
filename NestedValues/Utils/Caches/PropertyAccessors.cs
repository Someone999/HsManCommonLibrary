using System.Reflection;

namespace HsManCommonLibrary.NestedValues.Utils.Caches;

public class PropertyAccessors
{
    public MethodInfo? Getter { get; internal set; }
    public MethodInfo? Setter { get; internal set; }
}