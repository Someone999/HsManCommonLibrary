using System.Reflection;
using HsManCommonLibrary.Reflections.Finders;


namespace HsManCommonLibrary.Reflections;

public class TypeWrapper
{
    private Type _type;

    public TypeWrapper(Type type)
    {
        _type = type;
        _lazyConstructFinder = new Lazy<ConstructorFinder>(() => new ConstructorFinder(_type));
    }

    public bool IsAbstract => WrappedType.IsAbstract;
    public Type WrappedType => _type;
    
    public bool IsSubTypeOf(Type t)
    {
        return t.IsAssignableFrom(_type);
    }

    public bool IsSubTypeOf<T>() => IsSubTypeOf(typeof(T));
    public Type? GetFirstInheritedType(Type type) =>
        TypeInheritanceFinder.FindFirstInheritedType(_type, type);

    public Type? GetFirstInheritedGenericType(Type type) =>
        TypeGenericFinder.FindFirstInheritedGenericType(_type, type);

    private readonly Lazy<ConstructorFinder> _lazyConstructFinder;
    public ConstructorFinder GetConstructorFinder() => _lazyConstructFinder.Value;

    public MemberFinder<T> GetMemberFinder<T>() where T : MemberInfo => new(_type);
    public MethodFinder GetMethodFinder() => new MethodFinder(_type);
}