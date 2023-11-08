using System.Collections;

namespace HsManCommonLibrary.Reflections;

public class AssemblyTypeCollection : IEnumerable<Type>
{
    public AssemblyTypeCollection(IEnumerable<Type> types)
    {
        _types = new List<Type>(types);
    }
    private readonly List<Type> _types;
    public IEnumerator<Type> GetEnumerator() => _types.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public Type? GetTypeByName(string name)
    {
        return _types.FirstOrDefault(t => t.Name == name);
    }
        
    public Type? GetTypeByFullyQualifiedName(string name)
    {
        return _types.FirstOrDefault(t => t.FullName == name);
    }

    public Type[] GetSubTypesOf(Type t, bool ignoreNonPublic = true)
    {
        List<Type> matchedTypes = new List<Type>();
        foreach (var type in _types)
        {
            if (!t.IsAssignableFrom(type) || type.IsAbstract || type.IsInterface)
            {
                continue;
            }

            if (ignoreNonPublic && !type.IsPublic)
            {
                continue;
            }
                
            matchedTypes.Add(type);
        }

        return matchedTypes.ToArray();
    }

    public Type[] GetSubTypesOf<T>() => GetSubTypesOf(typeof(T));
}