using System.Collections.Concurrent;

namespace HsManCommonLibrary.NestedValues.Utils.Caches;

public class ConcurrentDictionaryWrapper<TKey, TValue> : DictionaryWrapper<TKey, TValue> where TKey : notnull
{
    public ConcurrentDictionaryWrapper(IDictionary<TKey, TValue> dictionary) : base(dictionary)
    {
        if (dictionary is not ConcurrentDictionary<TKey, TValue>)
        {
            throw new InvalidOperationException();
        }
    }

    public ConcurrentDictionaryWrapper() : this(new ConcurrentDictionary<TKey, TValue>())
    {
    }
    

    public override bool ContainsKey(TKey key)
    {
        return Dictionary.ContainsKey(key);
    }

    public override bool TryAdd(TKey key, TValue value)
    {
        return GetAs<ConcurrentDictionary<TKey, TValue>>().TryAdd(key, value);
    }

    public override bool TryGetValue(TKey key, out TValue? value) => Dictionary.TryGetValue(key, out value);
}