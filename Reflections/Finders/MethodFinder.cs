using System.Reflection;

namespace HsManCommonLibrary.Reflections.Finders;

public class MethodFinder : MemberFinder<MethodInfo>
{
    MethodInfo? FindMethodByTypes(IReadOnlyCollection<Type?> types)
    {
        var methods = InnerType.GetMethods();
        foreach (var method in methods)
        {
            var parameters = method.GetParameters();
            if (types.Count != parameters.Length)
            {
                continue;
            }

            int matched = types.Where((t, i) => t == null || t == parameters[i].ParameterType).Count();

            if (matched == types.Count)
            {
                return method;
            }
        }
        
        return null;
    }
    public MethodFinder(Type innerType) : base(innerType)
    {
    }

    private bool IsMatch(MethodFindOptions methodFindOptions, MethodInfo methodInfo)
    {
        if (methodFindOptions.CallingConventions != null && methodFindOptions.CallingConventions == methodInfo.CallingConvention)
        {
            return false;
        }
        

        if (methodFindOptions.ReturnType != null && methodInfo.ReturnType != methodFindOptions.ReturnType)
        {
            return false;
        }

        if (methodFindOptions.ParameterTypes != null)
        {
            var p = methodFindOptions.ParameterTypes;
            return !methodInfo.GetParameters().Where((t, i) => p[i] != null && p[i] != t.ParameterType).Any();
        }

        return true;
    }
    
    public MethodInfo? FindMember(MethodFindOptions methodFindOptions)
    {
        var bindingFlags = methodFindOptions.BindingFlags ?? BindingFlagsConstants.PublicMembers;
        var methods = FindMembers(methodFindOptions.MemberName, bindingFlags);
        return methods.Length == 0 
            ? null 
            : methods.FirstOrDefault(methodInfo => IsMatch(methodFindOptions, methodInfo));
    }
}