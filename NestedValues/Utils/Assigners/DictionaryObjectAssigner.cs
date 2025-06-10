using System.Collections;
using System.Reflection;
using HsManCommonLibrary.Exceptions;
using HsManCommonLibrary.NestedValues.Utils.Caches;
using HsManCommonLibrary.Reflections;

namespace HsManCommonLibrary.NestedValues.Utils.Assigners;

public class DictionaryObjectAssigner : IObjectAssigner
{
    public void Assign(PropertyInfo propertyInfo, object? ins, object? val, AssignOptions? assignOptions)
    {
        if (val == null)
        {
            throw new ArgumentNullException(nameof(val));
        }

        var propertySetter = PropertyCache.DefaultInstance.GetAccessors(propertyInfo);
        if (propertySetter == null)
        {
            throw new RecoverableException("The property has no setter.");
        }
        
        var propertyType = propertyInfo.PropertyType;
        
        var types = CollectionUtils.GetDictionaryGenericTypes(propertyType);
        var nestedVal = (INestedValueStore)val;
        var dict = CollectionUtils.CreateDictionaryFromNestedValue(types.ValueType, nestedVal, assignOptions);
        propertySetter.SetValue(ins, MatchDictionary(propertyType, dict));
    }
    
    static object? MatchDictionary(Type collectionType, IDictionary dictionary)
    {
        TypeWrapper wrapper = new TypeWrapper(collectionType);
        return wrapper.GetConstructorFinder().GetInstanceCreator().CreateInstance(new object?[] { dictionary });
    }
    
    public static IObjectAssigner ObjectAssigner { get; } = new DictionaryObjectAssigner();
}