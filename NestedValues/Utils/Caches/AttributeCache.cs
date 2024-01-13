using System.Reflection;

namespace HsManCommonLibrary.NestedValues.Utils.Caches;

public class AttributeCache
{
    private DictionaryWrapper<MemberInfo, AttributeCacheItem> _attributeCache =
        DictionaryWrapper<MemberInfo, AttributeCacheItem>.CreateCurrentUsing();

    private DictionaryWrapper<Attribute, Type?> _attributeTypeCache =
        DictionaryWrapper<Attribute, Type?>.CreateCurrentUsing();
    
    public bool HasAttribute(MemberInfo memberInfo, Type attributeType)
    {
        bool hasMember = _attributeCache.TryGetValue(memberInfo, out var attributeCacheItem);
        bool hasAttribute = attributeCacheItem?.AttributeCache.ContainsKey(attributeType) ?? false;
        return hasMember && hasAttribute;
    }
    
    public bool HasAttribute<TAttribute>(MemberInfo memberInfo) where TAttribute: Attribute
    {
        return HasAttribute(memberInfo, typeof(TAttribute));
    }

    public void AddItem(MemberInfo memberInfo, Attribute attribute)
    {
        _attributeCache.TryAdd(memberInfo, new AttributeCacheItem(memberInfo));
        if (!_attributeTypeCache.TryGetValue(attribute, out var attrType))
        {
            attrType = attribute.GetType();
            _attributeTypeCache.TryAdd(attribute, attrType);
        }

        if (attrType == null)
        {
            return;
        }
        
        _attributeCache[memberInfo].AttributeCache.TryAdd(attrType, new List<Attribute>());
        _attributeCache[memberInfo].AttributeCache[attrType].Add(attribute);
        
    }

    public Attribute? GetAttribute(MemberInfo memberInfo, Type attributeType)
    {
        return _attributeCache[memberInfo].GetAttribute(attributeType);
    }

    public TAttribute? GetAttribute<TAttribute>(MemberInfo memberInfo) where TAttribute: Attribute
    {
        return !_attributeCache.TryGetValue(memberInfo, out var attrs)
            ? null
            : attrs?.GetAttribute<TAttribute>();
    }

    public static AttributeCache DefaultInstance { get; } = new AttributeCache();
}