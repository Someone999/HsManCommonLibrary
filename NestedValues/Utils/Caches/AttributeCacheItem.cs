using System.Reflection;

namespace HsManCommonLibrary.NestedValues.Utils.Caches;

public class AttributeCacheItem
{
    public AttributeCacheItem(MemberInfo memberInfo)
    {
        MemberInfo = memberInfo;
    }

    public MemberInfo MemberInfo { get; private set; }

    public DictionaryWrapper<Type, List<Attribute>> AttributeCache { get; } =
        DictionaryWrapper<Type, List<Attribute>>.CreateCurrentUsing();
    
    public Attribute? GetAttribute(Type attr)
    {
        if (!AttributeCache.ContainsKey(attr))
        {
            return null;
        }

        var attrCount = AttributeCache[attr].Count;
        return attrCount switch
        {
            > 1 => throw new InvalidOperationException("Please use GetAttributes get multi-marked attribute"),
            0 => null,
            _ => AttributeCache[attr][0]
        };
    }

    public Attribute[] GetAttributes(Type attr)
    {
        return !AttributeCache.ContainsKey(attr) ? Array.Empty<Attribute>() : AttributeCache[attr].ToArray();
    }

    public TAttribute? GetAttribute<TAttribute>() where TAttribute : Attribute =>
        (TAttribute?)GetAttribute(typeof(TAttribute));
    
    public TAttribute[] GetAttributes<TAttribute>() where TAttribute : Attribute =>
        (TAttribute[])GetAttributes(typeof(TAttribute));
    
}