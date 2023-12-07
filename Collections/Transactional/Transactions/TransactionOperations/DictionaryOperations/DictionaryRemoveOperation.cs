namespace HsManCommonLibrary.Collections.Transactional.Transactions.TransactionOperations.DictionaryOperations;

public class DictionaryRemoveOperation<TKey, TValue> : ITransactionOperation<Dictionary<TKey, TValue>> where TKey: notnull
{
    private readonly TKey _key;

    public DictionaryRemoveOperation(TKey key)
    {
        _key = key;
    }

    private List<TransactionLog<Dictionary<TKey, TValue>>> _logs = new List<TransactionLog<Dictionary<TKey, TValue>>>();
    
    public IEnumerable<TransactionLog<Dictionary<TKey, TValue>>> CreateLogSet()
    {
        _logs.Add(new TransactionLog<Dictionary<TKey, TValue>>(this, null, null,
            OperationStatus.None));

        return _logs;
    }

    private bool _isCompleted;
    public bool Apply(Dictionary<TKey, TValue> collection)
    {
        try
        {
            if (!collection.TryGetValue(_key, out var val))
            {
                return false;
            }

            _isCompleted = collection.Remove(_key);

            _logs[0].OldValue = val;
            _logs[0].Status = _isCompleted ? OperationStatus.Success : OperationStatus.Failed;
            return _isCompleted;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool Undo(Dictionary<TKey, TValue> collection, TransactionLog<Dictionary<TKey, TValue>> transactionLog)
    {
        if (transactionLog.OldValue is not KeyValuePair<TKey, TValue> keyValuePair)
        {
            return false;
        }

        try
        {
            collection.Add(keyValuePair.Key, keyValuePair.Value);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
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