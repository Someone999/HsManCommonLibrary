using System.Reflection;

namespace HsManCommonLibrary.Reflections;

public class ConstructorFinder
{
    private readonly Type _type;

    public ConstructorFinder(Type type)
    {
        _type = type;
        _lazy = new Lazy<InstanceCreator>(() => new InstanceCreator(this));
    }

    public ConstructorInfo? GetConstructor(MethodFindOptions methodFindOptions)
    {
        var bindingFlags = methodFindOptions.BindingFlags ?? BindingFlags.Public | BindingFlags.Instance;
        var binder = methodFindOptions.Binder;
        var parameterTypes = methodFindOptions.ParameterTypes ?? Array.Empty<Type>();
        if (parameterTypes.Any(p => p == null))
        {
            return FindConstructorByTypes(parameterTypes);
        }

        var tmpParameterTypes = parameterTypes.Cast<Type>().ToArray();
        var callingConventions = methodFindOptions.CallingConventions ?? CallingConventions.Any;
        var parameterModifiers = methodFindOptions.ParameterModifiers;
        return _type.GetConstructor(bindingFlags, binder, callingConventions, tmpParameterTypes, parameterModifiers);
    }
    

    ConstructorInfo? FindConstructorByTypes(IReadOnlyCollection<Type?>? types)
    {
        if (types == null || types.Count == 0)
        {
            return GetConstructor(new MethodFindOptions());
        }

        var constructors = _type.GetConstructors();
        foreach (var constructor in constructors)
        {
            var parameters = constructor.GetParameters();
            if (types.Count != parameters.Length)
            {
                continue;
            }

            int matched = types.Where((t, i) => t == null || t == parameters[i].ParameterType).Count();

            if (matched == types.Count)
            {
                return constructor;
            }
        }
        
        return null;
    }

    private readonly Lazy<InstanceCreator> _lazy;
    public InstanceCreator GetInstanceCreator() => _lazy.Value;

}