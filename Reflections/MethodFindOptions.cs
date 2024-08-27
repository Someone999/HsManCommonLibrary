using System.Reflection;

namespace HsManCommonLibrary.Reflections;

public class MethodFindOptions
{
    public BindingFlags? BindingFlags { get; set; }
    public string MemberName { get; set; } = "";
    public Type?[]? ParameterTypes { get; set; }
    public Type? ReturnType { get; set; }
    public Binder? Binder { get; set; }
    public CallingConventions? CallingConventions { get; set; }
    public ParameterModifier[]? ParameterModifiers { get; set; }

    public static MethodFindOptions Empty { get; } = new MethodFindOptions();
}