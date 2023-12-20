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