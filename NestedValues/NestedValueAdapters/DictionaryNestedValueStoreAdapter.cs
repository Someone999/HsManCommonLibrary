using System.Collections;
using HsManCommonLibrary.Configuration;

namespace HsManCommonLibrary.NestedValues.NestedValueAdapters;

public class DictionaryNestedValueStoreAdapter : INestedValueStoreAdapter<Dictionary<string, object?>>
{
    Dictionary<string, object?> Expend(Dictionary<string, object?>? dictionary)
    {
        if(dictionary == null || dictionary.Count == 0)
        {
            return new Dictionary<string, object?>();
        }

        Dictionary<string, object?> configDictionary = new Dictionary<string, object?>();
        foreach (var pair in dictionary)
        {
            var key = pair.Key;
            var value = pair.Value;
            configDictionary.Add(key,
                IsCompatible(value?.GetType())
                    ? Expend(ConvertToStringObjectDictionary(value))
                    : value);
        }

        return configDictionary;
    }

    Dictionary<string, object?>? ConvertToStringObjectDictionary(object? obj)
    {
        switch (obj)
        {
            case null:
                return null;
            case Dictionary<string, object?> dictionary:
                return dictionary;
        }

        Type type = obj.GetType();
        if (!IsCompatible(type))
        {
            return null;
        }
        
        var enumerable = (IEnumerable)obj;
        Dictionary<string, object?> result = new Dictionary<string, object?>();
        foreach (var p in enumerable)
        {
            var pType = p.GetType();
            var key = pType.GetProperty("Key")?.GetValue(p);
            var value = pType.GetProperty("Value")?.GetValue(p) ?? null;

            if (key == null)
            {
                continue;
            }
            
            result.Add(key.ToString() ?? throw new InvalidOperationException("Failed to convert key"), 
                value ?? NullNestedValue.Value);
        }

        return result;
    }
    
    
    bool IsCompatible(Type? type)
    {
        while (true)
        {
            if (type == null)
            {
                return false;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                var genericArgs = type.GetGenericArguments();
                return genericArgs.Length != 2 || genericArgs[1] != typeof(INestedValueStore);
            }
            
            type = type.BaseType;
        }
    }

   
    
    INestedValueStore INestedValueStoreAdapter.ToNestedValue(object? obj)
    {
        if (obj == null)
        {
            return NullNestedValue.Value;
        }
        
        if (CanConvert(obj.GetType()) && obj is Dictionary<string, object?> dict)
        {
            return ToNestedValue(dict);
        }

        throw new InvalidOperationException();
    }

    public INestedValueStore ToNestedValue(Dictionary<string, object?>? obj)
    {
        if (!IsCompatible(obj?.GetType()))
        {
            throw new InvalidOperationException("Unable to convert the object to nested value store");
        }

        return new CommonNestedValueStore(Expend(ConvertToStringObjectDictionary(obj) ?? throw new InvalidOperationException()));
    }

    public bool CanConvert(Type t)
    {
        return IsCompatible(t);
    }

    public static INestedValueStoreAdapter Adapter { get; } = new DictionaryNestedValueStoreAdapter();
}