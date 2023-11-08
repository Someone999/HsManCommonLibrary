namespace HsManCommonLibrary.Collections.Transactional.Transactions.TransactionOperations;

public class ListClearOperation<T> : ITransactionOperation<List<T>>
{
    private readonly T[] _backupArray;
    List<TransactionLog<List<T>>> _logs = new List<TransactionLog<List<T>>>();

    public ListClearOperation(List<T> list)
    {
        _backupArray = list.ToArray();
    }
    
    public IEnumerable<TransactionLog<List<T>>> CreateLogSet()
    {
        return _logs;
    }

    public bool Apply(List<T> collection)
    {
        var tmpCollection = collection.ToArray();
        foreach (var item in tmpCollection)
        {
            var operation = new ListRemoveOperation<T>(item);
            var logSet = operation.CreateLogSet().ToArray();
            _logs.AddRange(logSet);
            var suc = operation.Apply(collection);
            if (!suc)
            {
                foreach (var transactionLog in logSet)
                {
                    transactionLog.MarkOperationFailed();
                }
                
                return false;
            }

            foreach (var transactionLog in logSet)
            {
                transactionLog.MarkOperationSuccess();
            }
        }
        
        return true;
    }

    public bool Undo(List<T> collection, TransactionLog<List<T>> transactionLog)
    {
        return false;
    }

    public object? GetOldValue()
    {
        return null;
    }

    public bool IsCompleted()
    {
        return true;
    }
}