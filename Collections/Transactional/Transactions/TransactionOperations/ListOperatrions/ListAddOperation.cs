namespace HsManCommonLibrary.Collections.Transactional.Transactions.TransactionOperations.ListOperatrions;

public class ListAddOperation<T> : ITransactionOperation<List<T>>
{
    private readonly T _value;
    private bool _transactionCompleted;

    public ListAddOperation(T value)
    {
        _value = value;
    }


    public IEnumerable<TransactionLog<List<T>>> CreateLogSet()
    {
        return new[] { new TransactionLog<List<T>>(this, null, _value, OperationStatus.Success) };
    }

    public bool Apply(List<T> collection)
    {
        collection.Add(_value);
        _transactionCompleted = true;
        return true;
    }

    public bool Undo(List<T> collection, TransactionLog<List<T>> transactionLog)
    {
        if (!IsCompleted())
        {
            throw new InvalidOperationException("Can not undo a uncompleted add operation.");
        }
        
        var newVal = (T?)transactionLog.NewValue;
        return newVal == null || collection.Remove(newVal);
    }

    public object? GetOldValue()
    {
        return null;
    }

    public bool IsCompleted() => _transactionCompleted;

}