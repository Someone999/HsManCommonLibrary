namespace HsManCommonLibrary.Reflections.Finders;

internal static class TypeInheritanceFinder
{
    public static Type? FindFirstInheritedType(Type type, Type targetType)
    {
        var inheritedTypes = GetAllInheritedTypes(type).ToArray();

        return inheritedTypes.FirstOrDefault(t => t == targetType) ??
               inheritedTypes.Select(t => FindFirstInheritedType(t, targetType)).FirstOrDefault(t => t != null);
    }

    private static IEnumerable<Type> GetAllInheritedTypes(Type type)
    {
        var inheritedTypes = type.GetInterfaces().ToList();
        if (type.BaseType != null && type.BaseType != typeof(object))
        {
            inheritedTypes.Add(type.BaseType);
        }
        return inheritedTypes;
    }
}