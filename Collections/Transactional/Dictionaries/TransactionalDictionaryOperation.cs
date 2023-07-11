namespace HsManCommonLibrary.Collections.Transactional.Dictionaries;

public class TransactionalDictionaryOperation<TKey, TValue>
{
    public TransactionalOperation Operation { get; set; }
    public TKey? Key { get; set; }
    public TValue? Value { get; set; }

    public static TransactionalDictionaryOperation<TKey, TValue> CreateAdd(TKey key, TValue value)
    {
        return new TransactionalDictionaryOperation<TKey, TValue>
        {
            Operation = TransactionalOperation.Add,
            Key = key,
            Value = value
        };
    }
        
    public static TransactionalDictionaryOperation<TKey, TValue> CreateRemove(TKey key)
    {
        return new TransactionalDictionaryOperation<TKey, TValue>
        {
            Operation = TransactionalOperation.Remove,
            Key = key,
        };
    }
        
    public static TransactionalDictionaryOperation<TKey, TValue> CreateClear()
    {
        return new TransactionalDictionaryOperation<TKey, TValue>
        {
            Operation = TransactionalOperation.Clear
        };
    }
}