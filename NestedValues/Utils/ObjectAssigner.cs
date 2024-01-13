using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using HsManCommonLibrary.Exceptions;
using HsManCommonLibrary.NameStyleConverters;
using HsManCommonLibrary.NestedValues.Attributes;
using HsManCommonLibrary.NestedValues.NestedValueConverters;
using HsManCommonLibrary.NestedValues.Utils.Caches;
using HsManCommonLibrary.Reflections;
using Newtonsoft.Json;

namespace HsManCommonLibrary.NestedValues.Utils;

public static class ObjectAssigner
{
    private static readonly object StaticLocker = new();
    public static PropertyCache PropertyCache => PropertyCache.DefaultInstance;

    public static void AssignTo(object? obj, INestedValueStore? nestedValueStore, AssignOptions? assignOptions)
    {
        if (obj == null || nestedValueStore == null)
        {
            return;
        }

        //Stopwatch stopwatch = new Stopwatch();
        //stopwatch.Start();
        var type = obj.GetType();
        
        PropertyCache.AddType(type);
        PropertyCache.CacheAllAccessors();


        //stopwatch.Stop();
        //Console.WriteLine($"GetProperties: {stopwatch.Elapsed}");
        //stopwatch.Reset();

        var properties = PropertyCache.GetProperties(type) ?? Array.Empty<PropertyInfo>();
        foreach (var property in properties)
        {
            //stopwatch.Start();
            AutoAssignAttribute? attr;
            attr = AttributeCache.DefaultInstance.GetAttribute<AutoAssignAttribute>(property);
            
            if (attr == null)
            {
                attr = property.GetCustomAttribute<AutoAssignAttribute>();
                if (attr != null)
                {
                    AttributeCache.DefaultInstance.AddItem(property, attr);
                }
            }
            
            
            if (attr == null)
            {
                AssignToNonAttribute(property, obj, nestedValueStore, assignOptions);
            }
            else
            {
                AssignToHasAttribute(nestedValueStore, property, obj, attr, assignOptions);
            }

            // stopwatch.Stop();
            // Console.WriteLine($"SetProperty: {stopwatch.Elapsed}");
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
        if (val == null || TypeCompareCache.DefaultInstance.GetIsCompatible(val, propertyInfo.PropertyType))
        {
            propertyInfo.SetValue(ins, val);
            return;
        }

        var propertySetter = PropertyCache.GetAccessors(propertyInfo)?.Setter;
        if (propertySetter == null)
        {
            return;
        }

        Type propertyType = propertyInfo.PropertyType;

        if (CollectionUtils.IsDictionary(propertyType))
        {
            var types = CollectionUtils.GetDictionaryGenericTypes(propertyType);
            var nestedVal = (INestedValueStore)val;
            var dict = CollectionUtils.CreateDictionaryFromNestedValue(types.ValueType, nestedVal, assignOptions);
            propertySetter.Invoke(ins, new[] { MatchDictionary(propertyType, dict) });
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
                propertySetter.Invoke(ins, new[] { currentVal });
                return;
            }

            if (TypeUtils.NeedToCreateNewObject(propertyType, nestedVal))
            {
                TypeUtils.TryCreateInstance(propertyType, nestedVal, assignOptions, out var obj);
                propertySetter.Invoke(ins, new[] { obj });
                return;
            }

            if (TypeCompareCache.DefaultInstance.GetIsCompatible(currentVal, propertyType))
            {
                propertySetter.Invoke(ins, new[] { currentVal });
                return;
            }

            if (assignOptions?.ConvertOptions.Converter != null)
            {
                currentVal = assignOptions.ConvertOptions.Converter.Convert(nestedVal);
                propertySetter.Invoke(ins, new[] { currentVal });
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

            propertySetter.Invoke(ins, new[] { currentVal });
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