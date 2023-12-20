using System.Collections;
using System.Reflection;
using HsManCommonLibrary.NameStyleConverters;
using HsManCommonLibrary.NestedValues.Attributes;
using HsManCommonLibrary.NestedValues.NestedValueConverters;
using HsManCommonLibrary.Reflections;

namespace HsManCommonLibrary.NestedValues.Utils;

public static class ObjectAssigner
{
    private static readonly object StaticLocker = new();
    public static void AssignTo(object? obj, INestedValueStore? nestedValueStore, AssignOptions? options)
    {
        lock (StaticLocker)
        {
            if (obj == null || nestedValueStore == null)
            {
                return;
            }
            
            var type = obj.GetType();
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var attr = property.GetCustomAttribute<AutoAssignAttribute>();
                if (attr == null)
                {
                    AssignForNonAttribute(nestedValueStore, property, obj, options);
                }
                else
                {
                    AssignForHasAttribute(nestedValueStore, property, obj, attr, options);
                }
            }
        }
    }

    static void AssignForNonAttribute(INestedValueStore nestedValueStore, PropertyInfo propertyInfo, object? ins
    , AssignOptions? options)
    {
        string propertyName = propertyInfo.Name;
        var val = nestedValueStore[propertyName];
        if (val == null)
        {
            var detectResult = NameStyleDetector.Detect(propertyName);
            if (detectResult != null)
            {
                propertyName = new LowerCamelCaseNameStyle().Normalize(detectResult.NameParts);
            }
            
            val = nestedValueStore[propertyName];
        }

        if (val == null)
        {
            return;
        }
        
        SetValue(ins, propertyInfo, val, options);
    }
    
    static void AssignForHasAttribute(INestedValueStore nestedValueStore, PropertyInfo propertyInfo,
        object? ins, AutoAssignAttribute autoAssignAttribute, AssignOptions? options)
    {
        var path = autoAssignAttribute.Path;
        if (string.IsNullOrEmpty(path))
        {
            AssignForNonAttribute(nestedValueStore, propertyInfo, ins, options);
            return;
        }
        
        if (autoAssignAttribute.IsNestedAssign)
        {
            AssignTo(ins, nestedValueStore, options);
        }
        else
        {
            INestedValueStoreConverter? converter = null;
            if (autoAssignAttribute.ConverterType != null)
            {
                var converterTypeWrapper = new TypeWrapper(autoAssignAttribute.ConverterType);
                object?[]? converterConstructorParams = null;
                options?.ConstructorParameters.TryGetValue(converterTypeWrapper.WrappedType,
                    out converterConstructorParams);

                converter =
                    converterTypeWrapper.CreateInstanceAs<INestedValueStoreConverter>(converterConstructorParams);
            }

            var originalVal = NestedValueStoreUtils.GetNestedValueStoreFromPath(nestedValueStore, path);
            object? val = converter == null
                ? originalVal
                : originalVal?.ConvertWith(converter);
            
            SetValue(ins, propertyInfo, val, options);
        }
        
    }

    static void SetValue(object? ins, PropertyInfo propertyInfo, object? val, 
        AssignOptions? options)
    {
        TypeWrapper propertyTypeWrapper = new TypeWrapper(propertyInfo.PropertyType);
        if (val is not INestedValueStore nestedValueStore)
        {
            propertyInfo.SetValue(ins, val);
            return;
        }
        
        if (propertyTypeWrapper.IsSubTypeOf<IDictionary>())
        {
            var dict = CreateDictionary(propertyInfo, nestedValueStore, options);
            propertyInfo.SetValue(ins, dict);
        }
        else if (propertyTypeWrapper.IsSubTypeOf<IEnumerable>())
        {
            var list = CreateCollection(propertyInfo, nestedValueStore, options);
            propertyInfo.SetValue(ins, list);
        }
        else
        {
            var currentVal = nestedValueStore.GetValue();
            if (currentVal != null && !propertyInfo.PropertyType.IsInstanceOfType(currentVal))
            {
                try
                {
                    var targetType = propertyInfo.PropertyType.IsEnum
                        ? Enum.GetUnderlyingType(propertyInfo.PropertyType)
                        : propertyInfo.PropertyType;
                    
                    currentVal = nestedValueStore.Convert(targetType);
                }
                catch (Exception e)
                {
                    throw new InvalidCastException(
                        $"Failed to cast {currentVal?.GetType()} to {propertyInfo.PropertyType}", e);
                }
            }
            
            propertyInfo.SetValue(ins, currentVal);
        }
    }
    
    static object? CreateCollection(PropertyInfo propertyInfo, INestedValueStore nestedValueStore, AssignOptions? options)
    {
        Type collectionType = propertyInfo.PropertyType;
        
        TypeWrapper collectionTypeWrapper = new TypeWrapper(collectionType);
        if (!collectionTypeWrapper.IsSubTypeOf(typeof(IEnumerable)))
        {
            throw new InvalidOperationException("The type is not enumerable.");
        }
        
        Type[] genericTypes = collectionType.GetGenericArguments();
        
        if (genericTypes.Length != 1 && !collectionType.IsArray)
        {
            throw new InvalidOperationException("This function can only process collections that have one generic arguments.");
        }

        Type genericType = collectionType.IsArray
            ? collectionType.GetElementType() ?? throw new InvalidOperationException()
            : genericTypes[0];
        var listType = typeof(List<>).MakeGenericType(genericType);
        TypeWrapper listTypeWrapper = new TypeWrapper(listType);
        var list = listTypeWrapper.CreateInstanceAs<IList>(null);
        if (list == null)
        {
            throw new InvalidCastException("Failed to build a list to build elements.");
        }
        
        var storedVal = nestedValueStore.GetValue();
        if (storedVal == null || storedVal.Equals(NullObject.Value) || storedVal is not IEnumerable enumerable)
        {
            return null;
        }

        TypeWrapper genericTypeWrapper = new TypeWrapper(genericType);
        object?[]? parameters = null;
        options?.ConstructorParameters.TryGetValue(genericType, out parameters);
        bool needInstantType = !genericTypeWrapper.IsSubTypeOf<IConvertible>();
        
        if (needInstantType && genericType == typeof(object))
        {
            throw new InvalidOperationException("The type 'object' is ambiguous for the constructors");
        }
        
        foreach (INestedValueStore nestedVal in enumerable)
        {
            if (needInstantType)
            {
                var genericObj = genericTypeWrapper.CreateInstance(parameters);
                AssignTo(genericObj, (INestedValueStore) nestedVal, null);
                list.Add(genericObj);
            }
            else
            {
                list.Add(nestedVal.Convert(genericType));
            }
           
        }

        return !collectionType.IsAssignableFrom(listType)
            ? MatchCollectionType(collectionType, list)
            : list;
    }

    static object? MatchCollectionType(Type collectionType, IEnumerable enumerable)
    {
        if (!collectionType.IsArray)
        {
            return new TypeWrapper(collectionType).CreateInstance(new object?[] { enumerable });
        }

        var genericElements = enumerable.Cast<object>();
        var genericElementList = genericElements.ToList();
        if (genericElementList.Count == 0)
        {
            return null;
        }

        var genericType = genericElementList[0].GetType();
        var arr = Array.CreateInstance(genericType, genericElementList.Count);
        for (int i = 0; i < genericElementList.Count; i++)
        {
            arr.SetValue(genericElementList[i], i);
        }

        return arr;
    }

    private static object? CreateDictionary(PropertyInfo propertyInfo, INestedValueStore nestedValueStore, AssignOptions? options)
    {
        Type collectionType = propertyInfo.PropertyType;
        TypeWrapper collectionTypeWrapper = new TypeWrapper(collectionType);
        if (!collectionTypeWrapper.IsSubTypeOf(typeof(IEnumerable)))
        {
            throw new InvalidOperationException("The type is not enumerable.");
        }
        
        Type[] genericTypes = collectionType.GetGenericArguments();
        
        if (genericTypes.Length != 2)
        {
            throw new InvalidOperationException("This function can only process collections that have one generic arguments.");
        }

        Type keyType = genericTypes[0];
        Type valType = genericTypes[1];

        
        bool valTypeNeedInstant = !typeof(IConvertible).IsAssignableFrom(valType);

        if (keyType != typeof(string))
        {
            throw new InvalidOperationException("Nested value can only process the string key");
        }
        
        if (valTypeNeedInstant && valType == typeof(object))
        {
            throw new InvalidOperationException("The type 'object' is ambiguous for the constructors");
        }

        var storedVal = nestedValueStore.GetValueAs<Dictionary<string, INestedValueStore>>();
        if (storedVal == null)
        {
            throw new InvalidCastException("Can not cast stored value to Dictionary<string, INestedValue>");
        }

        TypeWrapper valTypeWrapper = new TypeWrapper(valType);
        var dict = collectionTypeWrapper.CreateInstanceAs<IDictionary>(null);
        if (dict == null)
        {
            throw new InvalidCastException("Failed to build a dictionary to build elements.");
        }
        
        foreach (var pair in storedVal)
        {
            if (valTypeNeedInstant)
            {
                object?[]? constructorParams = null;
                options?.ConstructorParameters.TryGetValue(valType, out constructorParams);
                var val = valTypeWrapper.CreateInstance(constructorParams);
                AssignTo(val, pair.Value, null);
            }
            
            dict.Add(pair.Key, pair.Value.Convert(valType));
        }

        return !collectionType.IsInstanceOfType(dict)
            ? MatchDictionary(collectionType, dict)
            : dict;
    }

    static object? MatchDictionary(Type collectionType, IDictionary dictionary)
    {
        TypeWrapper wrapper = new TypeWrapper(collectionType);
        return wrapper.CreateInstance(new object?[] { dictionary });
    }
    
}