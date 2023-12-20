using System.Reflection;

namespace HsManCommonLibrary.PropertySynchronizer;

public class FailedProperty
{
    public FailedProperty(PropertyInfo propertyInfo, Exception? exception)
    {
        PropertyInfo = propertyInfo;
        Exception = exception;
    }

    public PropertyInfo PropertyInfo { get; }
    public Exception? Exception { get; }
}