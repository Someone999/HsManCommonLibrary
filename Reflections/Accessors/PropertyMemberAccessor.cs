using System.Reflection;

namespace HsManCommonLibrary.Reflections.Accessors;

public class PropertyMemberAccessor : IMemberAccessor
{
    private readonly PropertyInfo _propertyInfo;

    public PropertyMemberAccessor(PropertyInfo propertyInfo)
    {
        _propertyInfo = propertyInfo;
    }

    private void CheckParameters(MethodInfo methodInfo, object?[]? args)
    {

        var parameters = methodInfo.GetParameters();
        if (parameters.Length == 0)
        {
            return;
        }
        
        if (args == null || args.Length == 0)
        {
            var types = string.Join(",", parameters.Select(p =>  $"{p.Name}: {p.ParameterType}"));
            throw new InvalidOperationException($"Parameter(s) required. ({types})");
        }
        
        if (parameters.Length != args.Length)
        {
            var types = string.Join(",", parameters.Select(p =>  $"{p.Name}: {p.ParameterType}"));
            throw new InvalidOperationException($"Parameter(s) count mismatch. ({types})");
        }

        for (var i = 0; i < parameters.Length; i++)
        {
            var currentArg = args[i];
            // ReSharper disable once UseMethodIsInstanceOfType
            if (currentArg == null || parameters[i].ParameterType.IsAssignableFrom(currentArg.GetType()))
            {
                continue;
            }
            
            var expectedType = parameters[i].ParameterType;
            var actualType = currentArg.GetType();
            throw new InvalidOperationException($"Parameter type mismatch at index {i}. Expected: {expectedType}, Actual: {actualType}.");
        }
    }
    
    public object? GetValue(object? instance, params object?[]? args)
    {
        var getter = _propertyInfo.GetMethod;
        if (getter == null)
        {
            throw new InvalidOperationException("This member has no getter.");
        }
        
        CheckParameters(getter, args);
        return getter.Invoke(instance, args);
    }
    
    public void SetValue(object? instance, object? value, object?[]? args)
    {
        var setter = _propertyInfo.SetMethod;
        if (setter == null)
        {
            throw new InvalidOperationException("This member has no setter.");
        }

        var argCount = args?.Length ?? 0;

        object?[] obj = new object[argCount + 1];
        obj[0] = value;
        if (args != null)
        {
            Array.Copy(args, 0, obj, 1, args.Length);
        }
        
        CheckParameters(setter, args);
        setter.Invoke(instance, args);
    }
}