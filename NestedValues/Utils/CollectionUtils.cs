using System.Collections;
using HsManCommonLibrary.Reflections;

namespace HsManCommonLibrary.NestedValues.Utils;

public static class CollectionUtils
{
    public static Type GetCollectionGenericType(Type t)
    {
        if (t.IsArray)
        {
            return t.GetElementType() ?? throw new Exception("Failed to get type of element");
        }
        
        if (!IsCollection(t))
        {
            throw new InvalidOperationException("Not a collection type");
        }
        
        var genericType = new TypeWrapper(t).GetFirstInheritedGenericType(typeof(IEnumerable<>));
        return genericType ?? throw new Exception("Failed to get type of element");
    }
    
    public static IList CreateList(Type t)
    {
        var genericListType = typeof(List<>).MakeGenericType(t);
        TypeWrapper listTypeWrapper = new TypeWrapper(genericListType);
        return listTypeWrapper.CreateInstanceAs<IList>(null) ?? 
               throw new Exception("Failed to create List object");
    }
    
    public static IDictionary CreateDictionary(Type keyType, Type valType)
    {
        var genericDictionaryType = typeof(Dictionary<,>).MakeGenericType(keyType, valType);
        TypeWrapper dictTypeWrapper = new TypeWrapper(genericDictionaryType);
        return dictTypeWrapper.CreateInstanceAs<IDictionary>(null) ?? 
               throw new Exception("Failed to create Dictionary object");
    }

    public static IList CreateListFromNestedValueStore(Type elementType, INestedValueStore nestedValueStore, 
        AssignOptions? assignOptions)
    {
        var list = CreateList(elementType);
        if (nestedValueStore.GetValue() is not List<INestedValueStore> nestedValueStores)
        {
            throw new InvalidOperationException("Object in nested value store is not a list");
        }

        foreach (var storedVal in nestedValueStores)
        {
            var storedObj = storedVal.GetValue();
            if (TypeUtils.NeedToCreateNewObject(elementType, storedVal))
            {
                TypeUtils.TryCreateInstance(elementType, storedVal, assignOptions, out storedObj);
                ObjectAssigner.AssignTo(storedObj, storedVal, assignOptions);
                list.Add(storedObj);
                continue;
            }
            
            
            if (HsManCommonLibrary.Utils.TypeUtils.IsCompatibleType(storedObj, elementType))
            {
                throw new InvalidOperationException($"Can not convert object to type {elementType}");
            }

            var convertOptions = assignOptions?.ConvertOptions ?? new ConvertOptions();
            convertOptions.TargetType = elementType;
            if (!TryConvert(storedObj, convertOptions, out storedObj))
            {
                throw new InvalidOperationException($"Can not convert object to type {elementType}");
            }

            list.Add(storedObj);
        }

        return list;
    }

    public static IDictionary CreateDictionaryFromNestedValue(Type valType, 
        INestedValueStore nestedValueStore, AssignOptions? assignOptions)
    {
        var dictionary = CreateDictionary(typeof(string), valType);
        if (nestedValueStore.GetValue() is not Dictionary<string, INestedValueStore> nestedDictionary)
        {
            throw new InvalidOperationException("Object in nested value store is not a dictionary");
        }

        foreach (var storedVal in nestedDictionary)
        {
            var storedObj = storedVal.Value.GetValue();
            if (TypeUtils.NeedToCreateNewObject(valType, storedVal.Value))
            {
                TypeUtils.TryCreateInstance(valType, storedVal.Value, assignOptions, out storedObj);
                ObjectAssigner.AssignTo(storedObj, storedVal.Value, assignOptions);
                dictionary.Add(storedVal.Key, storedObj);
                continue;
            }
            
            if (HsManCommonLibrary.Utils.TypeUtils.IsCompatibleType(storedObj, valType))
            {
                throw new InvalidOperationException($"Can not convert object to type {valType}");
            }

            var convertOptions = assignOptions?.ConvertOptions ?? new ConvertOptions();
            convertOptions.TargetType = valType;
            if (!TryConvert(storedObj, convertOptions, out storedObj))
            {
                throw new InvalidOperationException($"Can not convert object to type {valType}");
            }

            dictionary.Add(storedVal.Key, storedObj);
        }

        return dictionary;
    }
    
    public static bool TryConvert(object? source, ConvertOptions convertOptions, out object? converted)
    {
        if (source == null)
        {
            converted = null;
            return true;
        }

        if (convertOptions.TargetType.IsInstanceOfType(source))
        {
            converted = source;
            return true;
        }

        if (convertOptions.AllowAutoAdapt)
        {
            var convertedObj = Convert.ChangeType(source, convertOptions.TargetType);
            converted = convertedObj;
            return true;
        }

        converted = null;
        return false;
    }

    

    public static DictionaryGenericTypeInfo GetDictionaryGenericTypes(Type dictionaryType)
    {
        TypeWrapper wrapper = new TypeWrapper(dictionaryType);
        Type genericDictionaryType = typeof(IDictionary<,>);
        Type? firstInheritedGenericType = wrapper.GetFirstInheritedGenericType(genericDictionaryType);
        
        if (firstInheritedGenericType == null)
        {
            throw new InvalidOperationException();
        }

        Type[] genericArguments = firstInheritedGenericType.GetGenericArguments();
        if (genericArguments.Length != 2)
        {
            throw new InvalidOperationException("Can not find generic type of this dictionary");
        }

        return new DictionaryGenericTypeInfo(genericArguments[0], genericArguments[1]);
    }

    public static HashSet<Type> ExcludeCollectionTypes { get; } = new HashSet<Type>()
    {
        typeof(string)
    };

    public static HashSet<Type> ExcludeDictionaryType { get; } = new HashSet<Type>()
    {
    };

    public static bool IsDictionary(Type t)
    {
        return !ExcludeDictionaryType.Contains(t) && typeof(IDictionary).IsAssignableFrom(t);
    }
    
    public static bool IsCollection(Type t)
    {
        return !ExcludeCollectionTypes.Contains(t) && typeof(IEnumerable).IsAssignableFrom(t);
    }
}