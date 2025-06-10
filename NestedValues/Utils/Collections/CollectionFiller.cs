using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using HsManCommonLibrary.Exceptions;
using HsManCommonLibrary.NestedValues.Utils.Assigners;
using HsManCommonLibrary.Reflections;

namespace HsManCommonLibrary.NestedValues.Utils.Collections;

public static class CollectionFiller
{
    public static void FillCollection(object obj, IEnumerable? enumerable)
    {
        if (enumerable == null)
        {
            return;
        }

        var type = obj.GetType();
        
        if (!CollectionUtils.IsCollection(type))
        {
            throw new InvalidOperationException($"Object {obj.GetType()} is not a collection");
        }

        if (type.IsArray)
        {
            FillArray(obj, enumerable);
            return;
        }
        
        TypeWrapper typeWrapper = new TypeWrapper(obj.GetType());
        var elementsType = typeWrapper.WrappedType.GetGenericArguments();
        if (elementsType.Length != 1)
        {
            throw new InvalidOperationException($"Object {obj.GetType()} is not a one generic argumnet collection");
        }

        MethodFindOptions options = new MethodFindOptions
        {
            MemberName = "Add",
            ParameterTypes = new[] { elementsType[0] },
        };
        
        var addMethod = typeWrapper.GetMethodFinder().FindMethod(options);
        if (addMethod == null)
        {
            throw new InvalidOperationException($"Object {obj.GetType()} has no generic add method");
        }

        foreach (var item in enumerable)
        {
            
            addMethod.Invoke(obj, new[] { item });
        }
    }

    private static void FillArray(object obj, IEnumerable? enumerable)
    {
        if (enumerable == null)
        {
            return;
        }
        
        var type = obj.GetType();
        if (!type.IsArray || obj is not Array arr)
        {
            throw new InvalidOperationException($"Object {obj.GetType()} is not an array");
        }

        if (arr.Rank != 1)
        {
            throw new NotSupportedException("Only single dimensional arrays are supported");
        }

        TypeWrapper typeWrapper = new TypeWrapper(typeof(Enumerable));
        var methodFinder = typeWrapper.GetMethodFinder();
        MethodFindOptions castOptions = new MethodFindOptions
        {
            MemberName = "Cast",
            ParameterTypes = new[] { typeof(IEnumerable) },
        };

        var castMethod = methodFinder.FindMethod(castOptions);
        if (castMethod == null)
        {
            throw new HsManInternalException();
        }

        var elementType = type.GetElementType();
        if (elementType == null)
        {
            throw new InvalidOperationException("Array element type is null");
        }
        
        MethodFindOptions toArrayOptions = new MethodFindOptions
        {
            MemberName = "ToArray",
            ParameterTypes = new[] { typeof(IEnumerable) },
        };
        
        var genericCastMethod = castMethod.MakeGenericMethod(elementType);
        var castResult = genericCastMethod.Invoke(null, new object?[] { enumerable });
        var toArrayMethod = methodFinder.FindMethod(toArrayOptions);
        if (toArrayMethod == null)
        {
            throw new HsManInternalException();
        }
        
        var genericToArrayMethod = toArrayMethod.MakeGenericMethod(elementType);
        var resultArray = genericToArrayMethod.Invoke(null, new[] { castResult });
        if (resultArray == null)
        {
            throw new InvalidOperationException("Failed to convert cast result to an array");
        }


        Array.Copy((Array)resultArray, arr, arr.Length);
    }
}