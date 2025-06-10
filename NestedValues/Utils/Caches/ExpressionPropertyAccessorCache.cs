using System.Linq.Expressions;
using System.Reflection;

namespace HsManCommonLibrary.NestedValues.Utils.Caches;

public class ExpressionPropertyAccessorCache : IPropertyAccessorCache
{
    private readonly PropertyInfo _propertyInfo;

    public ExpressionPropertyAccessorCache(PropertyInfo propertyInfo)
    {
        _propertyInfo = propertyInfo;
        CacheAll();
    }

    public Delegate? Getter { get; private set; }
    public Delegate? Setter { get; private set; }

    
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
        
        return Getter.DynamicInvoke(instance, parameters);
    }

    public void SetValue(object? instance, params object?[]? parameters)
    {
        if (Setter == null)
        {
            CacheGetter();
        }

        if (Setter == null)
        {
            throw new InvalidOperationException("Setter is null");
        }
        
        Setter.DynamicInvoke(instance, parameters);
    }
    public void CacheGetter()
    {
        var declaringType = _propertyInfo.DeclaringType;
        if (declaringType == null)
        {
            throw new InvalidOperationException("Can not compile getter for non-declaring type");
        }
        
        var indexerParameters = _propertyInfo.GetIndexParameters();
        if (indexerParameters.Length == 0)
        {
            CompileGetterNonIndexer(declaringType);
            return;
        }
        
        CompileGetterHasIndexer(declaringType, indexerParameters);
    }

    private void CompileGetterNonIndexer(Type declaringType)
    {
        var parameterExpression = Expression.Parameter(declaringType, "obj");
        var memberExpression = Expression.Property(parameterExpression, _propertyInfo);
        var lambda = Expression.Lambda(memberExpression, parameterExpression);
        Getter = lambda.Compile();
    }
    
    private void CompileGetterHasIndexer(Type declaringType, ParameterInfo[] indexerParameters)
    {
        var objParameterExpression = Expression.Parameter(declaringType, "obj");
       
        var getter = _propertyInfo.GetMethod;
        if (getter == null)
        {
            return;
        }

        var parameterExpressions = GetIndexerParameters(indexerParameters);
        var callExpression = Expression.Call(objParameterExpression, getter, parameterExpressions);
        
        var lambda = Expression.Lambda(callExpression, parameterExpressions);
        Getter = lambda.Compile();
    }

    List<ParameterExpression> GetIndexerParameters(ParameterInfo[] indexerParameters)
    {
        List<ParameterExpression> parameterExpressions = new List<ParameterExpression>();
        foreach (var parameterInfo in indexerParameters)
        {
            parameterExpressions.Add(Expression.Parameter(parameterInfo.ParameterType, parameterInfo.Name));
        }

        return parameterExpressions;
    }


    private void CompileSetterNonIndexer(Type declaringType)
    {
        var parameterExpression = Expression.Parameter(declaringType, "obj");
        var memberExpression = Expression.Property(parameterExpression, _propertyInfo);
        var valExpression = Expression.Parameter(_propertyInfo.PropertyType, "value");
        var lambda = Expression.Lambda(memberExpression, parameterExpression, valExpression);
        Setter = lambda.Compile();
    }
    
    private void CompileSetterHasIndexer(Type declaringType, ParameterInfo[] indexerParameters)
    {
        var objParameterExpression = Expression.Parameter(declaringType, "obj");
        var parameterExpressions = GetIndexerParameters(indexerParameters);
        parameterExpressions.Add(Expression.Parameter(_propertyInfo.PropertyType, "value"));
        var setter = _propertyInfo.SetMethod;
        if (setter == null)
        {
            return;
        }
        
        var callExpression = Expression.Call(objParameterExpression, setter, parameterExpressions);
        var lambda = Expression.Lambda(callExpression, parameterExpressions);
        Setter = lambda.Compile();
    }
    public void CacheSetter()
    {
        var declaringType = _propertyInfo.DeclaringType;
        if (declaringType == null)
        {
            throw new InvalidOperationException("Can not compile setter for non-declaring type");
        }

        var indexerParameters = _propertyInfo.GetIndexParameters();
        if (indexerParameters.Length == 0)
        {
            CompileSetterNonIndexer(declaringType);
            return;
        }
        
        CompileSetterHasIndexer(declaringType, indexerParameters);
    }

    public void CacheAll()
    {
        CacheGetter();
        CacheSetter();
    }

}