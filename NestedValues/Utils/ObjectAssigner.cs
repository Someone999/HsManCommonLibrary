using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using HsManCommonLibrary.Exceptions;
using HsManCommonLibrary.NameStyleConverters;
using HsManCommonLibrary.NestedValues.Attributes;
using HsManCommonLibrary.NestedValues.NestedValueConverters;
using HsManCommonLibrary.NestedValues.Utils.Assigners;
using HsManCommonLibrary.NestedValues.Utils.Caches;
using HsManCommonLibrary.NestedValues.Utils.Collections;
using HsManCommonLibrary.Reflections;
using Newtonsoft.Json;

namespace HsManCommonLibrary.NestedValues.Utils;

public static class ObjectAssigner
{
    public static PropertyCache PropertyCache => PropertyCache.DefaultInstance;

    public static void AssignTo(object? obj, INestedValueStore? nestedValueStore, AssignOptions? assignOptions)
    {
        if (obj == null || nestedValueStore == null)
        {
            return;
        }
        
        var type = obj.GetType();
        if (CollectionUtils.IsCollection(type))
        {
            CollectionFiller.FillCollection(obj, (IEnumerable?) nestedValueStore.GetValue());
            return;
        }
        
        PropertyCache.AddType(type);
        PropertyCache.CacheAllAccessors();
        
        var properties = PropertyCache.GetProperties(type) ?? Array.Empty<PropertyInfo>();
        foreach (var property in properties)
        {
            var attr = AttributeCache.DefaultInstance.GetAttribute<AutoAssignAttribute>(property);
            
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
                propertyName = LowerCamelCaseNameStyle.Instance.Normalize(detectResult.NameParts);
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
                    converterTypeWrapper.GetConstructorFinder()
                        .GetInstanceCreator()
                        .CreateInstanceAs<INestedValueStoreConverter>(converterConstructorParams);
                assignOptions ??= new AssignOptions();
                assignOptions.ConvertOptions.Converter = converter;
            }

            var originalVal = NestedValueStoreUtils.GetNestedValueStoreFromPath(nestedValueStore, path);
            SetValue(propertyInfo, ins, originalVal, assignOptions);
        }
    }

    private static void SetValue(PropertyInfo propertyInfo, object? ins, object? val, AssignOptions? assignOptions)
    {
        if (val == null)
        {
            NullObjectAssigner.ObjectAssigner.Assign(propertyInfo, ins, val, assignOptions);
            return;
        }
        
        Type propertyType = propertyInfo.PropertyType;

        if (CollectionUtils.IsDictionary(propertyType))
        {
           DictionaryObjectAssigner.ObjectAssigner.Assign(propertyInfo, ins, val, assignOptions);
        }
        else if (CollectionUtils.IsCollection(propertyInfo.PropertyType))
        {
            CollectionObjectAssigner.ObjectAssigner.Assign(propertyInfo, ins, val, assignOptions);
        }
        else
        {
            GeneralObjectAssigner.ObjectAssigner.Assign(propertyInfo, ins, val, assignOptions);
        }
    }
}