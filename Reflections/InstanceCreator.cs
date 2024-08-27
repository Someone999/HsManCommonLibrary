namespace HsManCommonLibrary.Reflections;

public class InstanceCreator
{
    private readonly ConstructorFinder _constructorFinder;

    public InstanceCreator(ConstructorFinder constructorFinder)
    {
        _constructorFinder = constructorFinder;
    }

    public object? CreateInstance(object?[]? parameters)
    {
        var constructorInfo = _constructorFinder.GetConstructor(new MethodFindOptions());

        if (constructorInfo == null)
        {
            throw new MissingMethodException();
        }

        if (parameters == null)
        {
            return constructorInfo.Invoke(Array.Empty<object>());
        }

        return constructorInfo.Invoke(parameters);
    }

    public T? CreateInstanceAs<T>(object?[]? parameters) => (T?)CreateInstance(parameters);
}