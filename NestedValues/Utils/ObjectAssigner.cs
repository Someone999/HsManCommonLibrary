using System.Reflection;
using HsManCommonLibrary.NestedValues.Attributes;

namespace HsManCommonLibrary.NestedValues.Utils;

public static class NestedValueStoreUtils
{
    
    public static INestedValueStore? GetNestedValueStoreFromPath(INestedValueStore nestedValueStore, string path)
    {
        return GetNestedValueStoreFromPath(nestedValueStore, path.Split('.'));
    }
    
    public static INestedValueStore? GetNestedValueStoreFromPath(INestedValueStore nestedValueStore, string[] paths)
    {
        var currentNestedValue = nestedValueStore;
        foreach (var path in paths)
        {
            if (currentNestedValue == null)
            {
                return null;
            }
            
            currentNestedValue = currentNestedValue[path];
        }

        return currentNestedValue;
    }
    
    public static object? GetValueFromPath(INestedValueStore nestedValueStore, string path)
    {
        return GetValueFromPath(nestedValueStore, path.Split('.'));
    }
    
    public static object? GetValueFromPath(INestedValueStore nestedValueStore, string[] paths)
    {
        return GetNestedValueStoreFromPath(nestedValueStore, paths)?.GetValue();
    }
    
}


public static class ObjectAssigner
{
    private static readonly object StaticLocker = new();
    public static void AssignTo(object? obj, INestedValueStore? nestedValueStore)
    {
        lock (StaticLocker)
        {
            if (obj == null || nestedValueStore == null)
            {
                return;
            }
        
            Type t = obj.GetType();
            var properties = t.GetProperties();
            foreach (var property in properties)
            {
                var autoAssignAttribute = property.GetCustomAttribute<AutoAssignAttribute>();
                if (autoAssignAttribute == null || string.IsNullOrEmpty(autoAssignAttribute.Path))
                {
                    continue;
                }
            
                if (autoAssignAttribute.IsNestedAssign)
                {
                    var currentVal =
                        NestedValueStoreUtils.GetNestedValueStoreFromPath(nestedValueStore, autoAssignAttribute.Path);
                    if (currentVal == null)
                    {
                        throw new KeyNotFoundException();
                    }
                
                    AssignTo(property.GetValue(obj), currentVal);
                }
                else
                {
                    var currentVal = NestedValueStoreUtils.GetValueFromPath(nestedValueStore, autoAssignAttribute.Path);
                    
                    if (currentVal != null && !property.PropertyType.IsInstanceOfType(currentVal))
                    {
                        throw new InvalidCastException(
                            $"Failed to cast {currentVal.GetType()} to {property.PropertyType}");
                    }
                    
                    property.SetValue(obj, currentVal);
                }
            }
        }
    }
}