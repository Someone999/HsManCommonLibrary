using System.Collections;
using System.Configuration;
using System.Reflection;
using System.Runtime.InteropServices;
using HsManCommonLibrary.NameStyleConverters;
using HsManCommonLibrary.NestedValues.Attributes;
using HsManCommonLibrary.NestedValues.NestedValueConverters;
using HsManCommonLibrary.Reflections;
using Newtonsoft.Json;

namespace HsManCommonLibrary.NestedValues.Utils;

public static class ObjectAssigner
{
    private static readonly object StaticLocker = new();
    public static void AssignTo(object? obj, INestedValueStore? nestedValueStore, AssignOptions? assignOptions)
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
                    AssignToNonAttribute(property, obj, nestedValueStore, assignOptions);
                }
                else
                {
                    AssignToHasAttribute(nestedValueStore, property, obj, attr, assignOptions);
                }
            }
        }
    }

    private static void AssignToNonAttribute(PropertyInfo propertyInfo, object? ins, INestedValueStore nestedValueStore,
        AssignOptions? assignOptions)
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
        
        SetValue(propertyInfo, ins, val, assignOptions);
    }
    
    private static void AssignToHasAttribute(INestedValueStore nestedValueStore, PropertyInfo propertyInfo,
        object? ins, AutoAssignAttribute autoAssignAttribute, AssignOptions? assignOptions)
    {
        var path = autoAssignAttribute.Path;
        if (string.IsNullOrEmpty(path))
        {
            AssignToNonAttribute(propertyInfo, ins, nestedValueStore, assignOptions);
            return;
        }
        
        if (autoAssignAttribute.IsNestedAssign)
        {
            AssignTo(ins, nestedValueStore, assignOptions);
        }
        else
        {
            if (autoAssignAttribute.ConverterType != null)
            {
                var converterTypeWrapper = new TypeWrapper(autoAssignAttribute.ConverterType);
                object?[]? converterConstructorParams = null;
                assignOptions?.ConstructorParameters.TryGetValue(converterTypeWrapper.WrappedType,
                    out converterConstructorParams);

                var converter = 
                    converterTypeWrapper.CreateInstanceAs<INestedValueStoreConverter>(converterConstructorParams);
                assignOptions ??= new AssignOptions();
                assignOptions.ConvertOptions.Converter = converter;
            }

            var originalVal = NestedValueStoreUtils.GetNestedValueStoreFromPath(nestedValueStore, path);
            SetValue(propertyInfo, ins, originalVal, assignOptions);
        }
    }

    private static void SetValue(PropertyInfo propertyInfo, object? ins, object? val, AssignOptions? assignOptions)
    {
        if (val == null || HsManCommonLibrary.Utils.TypeUtils.IsCompatibleType(val, propertyInfo.PropertyType))
        {
            propertyInfo.SetValue(ins, val);
            return;
        }
        
        Type propertyType = propertyInfo.PropertyType;

        if (CollectionUtils.IsDictionary(propertyType))
        {
            var types = CollectionUtils.GetDictionaryGenericTypes(propertyType);
            var nestedVal = (INestedValueStore)val;
            var dict = CollectionUtils.CreateDictionaryFromNestedValue(types.ValueType, nestedVal, assignOptions);
            propertyInfo.SetValue(ins, MatchDictionary(propertyType, dict));
        }
        else if (CollectionUtils.IsCollection(propertyInfo.PropertyType))
        {
            var elementType = CollectionUtils.GetCollectionGenericType(propertyType);
            var nestedVal = (INestedValueStore)val;
            var list = CollectionUtils.CreateListFromNestedValueStore(elementType, nestedVal, assignOptions);
            propertyInfo.SetValue(ins, MatchCollection(propertyType, list));
        }
        else
        {
            var nestedVal = (INestedValueStore)val;
            var currentVal = nestedVal.GetValue();
            if (NullNestedValue.Value.Equals(currentVal))
            {
                currentVal = default;
                propertyInfo.SetValue(ins,  currentVal);
                return;
            }

            if (TypeUtils.NeedToCreateNewObject(propertyType, nestedVal))
            {
                TypeUtils.TryCreateInstance(propertyType, nestedVal, assignOptions, out var obj);
                propertyInfo.SetValue(ins, obj);
                return;
            }
            
            if (HsManCommonLibrary.Utils.TypeUtils.IsCompatibleType(currentVal, propertyType))
            {
                propertyInfo.SetValue(ins,  currentVal);
                return;
            }
            
            if (assignOptions?.ConvertOptions.Converter != null)
            {
                currentVal = assignOptions.ConvertOptions.Converter.Convert(nestedVal);
                propertyInfo.SetValue(ins, currentVal);
                return;
            }
           
            var allowAutoAdapt = assignOptions?.ConvertOptions.AllowAutoAdapt ?? true;
            if (allowAutoAdapt && currentVal is IConvertible)
            {
                if (propertyType.IsEnum)
                {
                    propertyType = Enum.GetUnderlyingType(propertyType);
                }
                
                currentVal = Convert.ChangeType(currentVal, propertyType);
            }
            else
            {
                throw new InvalidOperationException("Auto-adapt is disallowed");
            }
            
            propertyInfo.SetValue(ins, currentVal);
        }
    }

    
    static object? MatchDictionary(Type collectionType, IDictionary dictionary)
    {
        TypeWrapper wrapper = new TypeWrapper(collectionType);
        return wrapper.CreateInstance(new object?[] { dictionary });
    }

    static object? MatchCollection(Type collectionType, IEnumerable enumerable)
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
    
    
}