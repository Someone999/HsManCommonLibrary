namespace HsManCommonLibrary.NestedValues.Utils.Caches;

public class NormalDictionaryWrapper<TKey, TValue> : DictionaryWrapper<TKey, TValue> where TKey : notnull
{
    public NormalDictionaryWrapper(IDictionary<TKey, TValue> dictionary) : base(dictionary)
    {
        if (dictionary is not Dictionary<TKey, TValue>)
        {
            throw new InvalidOperationException();
        }
    }

    public NormalDictionaryWrapper() : this(new Dictionary<TKey, TValue>())
    {
    }

    public override bool ContainsKey(TKey key)
    {
        return Dictionary.ContainsKey(key);
    }

    public override bool TryAdd(TKey key, TValue value)
    {
        if (Dictionary.ContainsKey(key))
        {
            return false;
        }
        
        Dictionary.Add(key, value);
        return true;
    }

    public override bool TryGetValue(TKey key, out TValue? value) => Dictionary.TryGetValue(key, out value);
}