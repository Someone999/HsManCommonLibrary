using System.Collections;
using System.Reflection;
using HsManCommonLibrary.Reflections;

namespace HsManCommonLibrary.NestedValues.Utils.Assigners;

public class CollectionObjectAssigner : IObjectAssigner
{
    public void Assign(PropertyInfo propertyInfo, object? ins, object? val, AssignOptions? assignOptions)
    {
        if (val == null)
        {
            throw new ArgumentNullException(nameof(val));
        }
        
        var propertyType = propertyInfo.PropertyType;
        var elementType = CollectionUtils.GetCollectionGenericType(propertyType);
        var nestedVal = (INestedValueStore)val;
        var list = CollectionUtils.CreateListFromNestedValueStore(elementType, nestedVal, assignOptions);
        propertyInfo.SetValue(ins, MatchCollection(propertyType, list));
    }

    private static object? MatchCollection(Type collectionType, IEnumerable enumerable)
    {
        if (!collectionType.IsArray)
        {
            var elementTypes = collectionType.GetGenericArguments();
            if (elementTypes.Length != 1)
            {
                throw new InvalidOperationException("Collection has more 1 element types");
            }
            
            MethodFindOptions options = new MethodFindOptions()
            {
                ParameterTypes = new[] { typeof(IEnumerable<>).MakeGenericType(elementTypes[0]) }
            };
            return new TypeWrapper(collectionType)
                .GetConstructorFinder()
                .GetInstanceCreator()
                .CreateInstance(options, new object?[] { enumerable });
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
    
    public static IObjectAssigner ObjectAssigner { get; } = new CollectionObjectAssigner();
}