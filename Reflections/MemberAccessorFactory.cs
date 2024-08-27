using System.Reflection;
using HsManCommonLibrary.Exceptions;
using HsManCommonLibrary.Reflections.Accessors;

namespace HsManCommonLibrary.Reflections;

public static class MemberAccessorFactory
{
    public static T Create<T>(MemberInfo memberInfo) where T: IMemberAccessor
    {
        IMemberAccessor accessor = memberInfo switch
        {
            PropertyInfo propertyInfo => new PropertyMemberAccessor(propertyInfo),
            FieldInfo fieldInfo => new FieldMemberAccessor(fieldInfo),
            _ => throw new InvalidOperationException()
        };

        return (T)accessor;
    }
}