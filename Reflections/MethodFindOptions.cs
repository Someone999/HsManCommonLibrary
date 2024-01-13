using System.Reflection;

namespace HsManCommonLibrary.Reflections;

public class MethodFindOptions : MemberFindOptions
{
    public Type?[]? ParameterTypes { get; set; }
    public Type? ReturnType { get; set; }
    public Binder? Binder { get; set; }
    public CallingConventions? CallingConventions { get; set; }
    public ParameterModifier[]? ParameterModifiers { get; set; }

    public new static MethodFindOptions Empty { get; } = new MethodFindOptions();
}