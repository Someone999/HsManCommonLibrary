namespace HsManCommonLibrary.NestedValues.Utils.Caches;

public abstract class DictionaryWrapper<TKey, TValue> where TKey : notnull
{
    public IDictionary<TKey, TValue> Dictionary { get; }
    
    
    public DictionaryWrapper(IDictionary<TKey, TValue> dictionary)
    {
        Dictionary = dictionary;
    }

    public abstract bool ContainsKey(TKey key);
    public abstract bool TryAdd(TKey key, TValue value);
    public abstract bool TryGetValue(TKey key, out TValue? value);

    public virtual TDictionary GetAs<TDictionary>() where TDictionary : IDictionary<TKey, TValue>
    {
        return (TDictionary)Dictionary;
    }

    public TValue this[TKey key]
    {
        get => Dictionary[key];
        set => Dictionary[key] = value;
    }

    public static DictionaryWrapper<TKey, TValue> CreateCurrentUsing()
    {
        return new ConcurrentDictionaryWrapper<TKey, TValue>();
    }
}    