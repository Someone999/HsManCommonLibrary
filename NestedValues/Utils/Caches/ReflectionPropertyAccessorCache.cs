using System.Reflection;

namespace HsManCommonLibrary.NestedValues.Utils.Caches;

public class ReflectionPropertyAccessorCache : IPropertyAccessorCache
{
    private readonly PropertyInfo _propertyInfo;

    public ReflectionPropertyAccessorCache(PropertyInfo propertyInfo)
    {
        _propertyInfo = propertyInfo;
        CacheAll();
    }
     MethodInfo? Getter { get; set; }
     MethodInfo? Setter { get; set; }

    public object? GetValue(object? instance, params object?[]? parameters)
    {
        if (Getter == null)
        {
            CacheGetter();
        }

        if (Getter == null)
        {
            throw new InvalidOperationException("Getter is null");
        }
        
        return Getter.Invoke(instance, parameters);
    }

    public void SetValue(object? instance, params object?[]? parameters)
    {
        if (Setter == null)
        {
            CacheSetter();
        }

        if (Setter == null)
        {
            throw new InvalidOperationException("Setter is null");
        }
        
        Setter.Invoke(instance, parameters);
    }

    public void CacheGetter()
    {
        Getter = _propertyInfo.GetGetMethod(true);
    }
    
    public void CacheSetter()
    {
        Setter = _propertyInfo.GetSetMethod(true);
    }
    
    public void CacheAll()
    {
        CacheGetter();
        CacheSetter();
    }
}