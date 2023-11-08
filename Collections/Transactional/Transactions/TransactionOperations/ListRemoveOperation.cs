namespace HsManCommonLibrary.Collections.Transactional.Transactions.TransactionOperations;

public class ListRemoveOperation<T> : ITransactionOperation<List<T>>
{
    private readonly T _obj;
    private bool _transactionCompleted;
    private List<TransactionLog<List<T>>> _logs = new List<TransactionLog<List<T>>>();

    public ListRemoveOperation(T obj)
    {
        _obj = obj;
    }
    public IEnumerable<TransactionLog<List<T>>> CreateLogSet()
    {
        _logs.Add(new TransactionLog<List<T>>(this, _obj, null, OperationStatus.None));
        return _logs;
    }

    public bool Apply(List<T> collection)
    {
        _transactionCompleted = true;
        
        return collection.Remove(_obj);
    }

    public bool Undo(List<T> collection, TransactionLog<List<T>> transactionLog)
    {
        var oldVal = (T?)transactionLog.OldValue;
        if (oldVal == null)
        {
            return false;
        }
        
        collection.Add(oldVal);
        return true;
    }

    public object? GetOldValue()
    {
        return _obj;
    }

    public bool IsCompleted()
    {
        return _transactionCompleted;
    }
}