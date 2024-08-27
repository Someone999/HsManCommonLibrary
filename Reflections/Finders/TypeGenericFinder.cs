namespace HsManCommonLibrary.Reflections.Finders;

internal static class TypeGenericFinder
{
    public static Type? FindFirstInheritedGenericType(Type type, Type targetGenericType)
    {
        var inheritedTypes = GetAllInheritedTypes(type).ToArray();

        return inheritedTypes.FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == targetGenericType) ??
               inheritedTypes.Select(t => FindFirstInheritedGenericType(t, targetGenericType)).FirstOrDefault(t => t != null);
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