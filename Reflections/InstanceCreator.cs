namespace HsManCommonLibrary.Reflections;

public class InstanceCreator
{
    private readonly ConstructorFinder _constructorFinder;

    public InstanceCreator(ConstructorFinder constructorFinder)
    {
        _constructorFinder = constructorFinder;
    }

    public object? CreateInstance(object?[]? arguments)
    {
        var constructorInfo = _constructorFinder.GetConstructor(new MethodFindOptions());

        if (constructorInfo == null)
        {
            throw new MissingMethodException();
        }

        return constructorInfo.Invoke(arguments ?? Array.Empty<object>());
    }

    public object? CreateInstance(MethodFindOptions findOptions, object?[]? arguments)
    {
        var constructorInfo = _constructorFinder.GetConstructor(findOptions);
        if (constructorInfo == null)
        {
            throw new MissingMethodException();
        }

        return constructorInfo.Invoke(arguments ?? Array.Empty<object>());
    }

    public T? CreateInstanceAs<T>(object?[]? parameters) => (T?)CreateInstance(parameters);
}