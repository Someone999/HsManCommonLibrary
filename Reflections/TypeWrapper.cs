using System.Reflection;


namespace HsManCommonLibrary.Reflections;

public class TypeWrapper
{
    private Type _type;

    public TypeWrapper(Type type)
    {
        _type = type;
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
    
    MethodInfo? FindMethodByTypes(IReadOnlyCollection<Type?>? types)
    {
        if (types == null || types.Count == 0)
        {
            return FindMethod(new MethodFindOptions());
        }

        var methods = _type.GetMethods();
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

    public object? CreateInstance(object?[]? parameters)
    {
        if (parameters == null)
        {
            return GetConstructor(new MethodFindOptions())?.Invoke(Array.Empty<object>());
        }

        ConstructorInfo? constructorInfo;
        if (parameters.Any(p => p == null))
        {
            var parameterTypes = parameters.Select(p => p?.GetType()).ToArray();
            constructorInfo = FindConstructorByTypes(parameterTypes);
        }
        else
        {
            var parameterTypes = parameters.Cast<object>().Select(o => o.GetType()).ToArray();
            constructorInfo = GetConstructor(new MethodFindOptions(){ParameterTypes = parameterTypes});
        }

        if (constructorInfo == null)
        {
            throw new MissingMethodException();
        }
        
        return constructorInfo.Invoke(parameters);
    }

    public T? CreateInstanceAs<T>(object?[]? parameters) => (T?)CreateInstance(parameters);

    public void SetMemberValue(MemberFindOptions findOptions, object? ins, object? val)
    {
        var bindingFlags = findOptions.BindingFlags ?? BindingFlags.Public | BindingFlags.Instance;
        
        
        var field = _type.GetField(findOptions.MemberName, bindingFlags);
        if (field != null)
        {
            field.SetValue(ins, val);
        }

        var memberName = findOptions.MemberName;
        
        var property = _type.GetProperty(memberName, bindingFlags);

        if (property == null)
        {
            throw new MissingMemberException($"Can not find member named \"{memberName}\" in type {_type}");
        }
        
        
        
        property.SetValue(ins, val);
    }
    public object? GetMemberValue(MemberFindOptions findOptions, object? ins)
    {
        var bindingFlags = findOptions.BindingFlags ?? BindingFlags.Public | BindingFlags.Instance;
        var field = _type.GetField(findOptions.MemberName, bindingFlags);
        if (field != null)
        {
            return field.GetValue(ins);
        }
        
        var memberName = findOptions.MemberName;
        var property = _type.GetProperty(memberName, bindingFlags);

        if (property == null)
        {
            throw new MissingMemberException($"Can not find member named \"{memberName}\" in type {_type}");
        }

        return property.GetValue(ins);
    }

    public T? GetMemberValueAs<T>(MemberFindOptions findOptions, object? ins) => (T?)GetMemberValue(findOptions, ins);
    

    public MethodInfo? FindMethod(MethodFindOptions methodFindOptions)
    {
        var methodName = methodFindOptions.MemberName;
        var bindingFlags = methodFindOptions.BindingFlags ?? BindingFlags.Public | BindingFlags.Instance;
        if (methodFindOptions.ParameterTypes == null)
        {
            return _type.GetMethod(methodName, bindingFlags);
        }
        
        var binder = methodFindOptions.Binder;
        var parameterTypes = methodFindOptions.ParameterTypes ?? Array.Empty<Type>();
        if (parameterTypes.Any(p => p == null))
        {
            return FindMethodByTypes(parameterTypes);
        }

        var tmpParameterTypes = parameterTypes.Cast<Type>().ToArray();
        var callingConventions = methodFindOptions.CallingConventions ?? CallingConventions.Any;
        var parameterModifiers = methodFindOptions.ParameterModifiers;


        MethodInfo? methodInfo = _type.GetMethod(methodName, bindingFlags, binder, callingConventions,
            tmpParameterTypes,
            parameterModifiers);

        return methodInfo;
    }

    public object? InvokeMethod(MethodFindOptions methodFindOptions,object? ins, object?[]? args)
    {
        return FindMethod(methodFindOptions)?.Invoke(ins, args);
    }
}