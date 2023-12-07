namespace HsManCommonLibrary.Collections.Transactional.Transactions.TransactionOperations.DictionaryOperations;

public class DictionaryAddOperation<TKey, TValue> : ITransactionOperation<Dictionary<TKey, TValue>> where TKey: notnull
{
    private readonly TKey _key;
    private readonly TValue _value;

    public DictionaryAddOperation(TKey key, TValue value)
    {
        _key = key;
        _value = value;
    }

    private List<TransactionLog<Dictionary<TKey, TValue>>> _logs = new List<TransactionLog<Dictionary<TKey, TValue>>>();
    
    public IEnumerable<TransactionLog<Dictionary<TKey, TValue>>> CreateLogSet()
    {
        _logs.Add(new TransactionLog<Dictionary<TKey, TValue>>(this, null, new KeyValuePair<TKey, TValue>(_key, _value),
            OperationStatus.None));

        return _logs;
    }

    private bool _isCompleted;
    public bool Apply(Dictionary<TKey, TValue> collection)
    {
        try
        {
            collection.Add(_key, _value);
            _logs[0].Status = OperationStatus.Success;
            return _isCompleted = true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool Undo(Dictionary<TKey, TValue> collection, TransactionLog<Dictionary<TKey, TValue>> transactionLog)
    {
        return transactionLog.NewValue is KeyValuePair<TKey, TValue> keyValuePair && collection.Remove(keyValuePair.Key);
    }

    public object? GetOldValue()
    {
        return null;
    }

    public bool IsCompleted()
    {
        return _isCompleted;
    }
}