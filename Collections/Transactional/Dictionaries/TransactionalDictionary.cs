using HsManCommonLibrary.Exceptions;

namespace HsManCommonLibrary.Collections.Transactional.Dictionaries;

public class TransactionalDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TKey: notnull
{
    private List<TransactionalDictionaryOperation<TKey, TValue>> _pendingOperations =
        new List<TransactionalDictionaryOperation<TKey, TValue>>();

    private readonly object _locker = new object();

    public void AddTransactionalOperation(TransactionalDictionaryOperation<TKey, TValue> operation)
    {
        lock (_locker)
        {
            _pendingOperations.Add(operation);
        }
    }
        
    public void Commit()
    {

        lock (_locker)
        {
            foreach (var operation in _pendingOperations)
            {
                switch (operation.Operation)
                {
                    case TransactionalOperation.Add:
                        if (operation.Key == null || operation.Value == null)
                        {
                            throw new InvalidTransactionalOperationException("Key and value can not be null");
                        }
                    
                        Add(operation.Key, operation.Value);
                        break;
                    case TransactionalOperation.Remove:
                        if (operation.Key == null)
                        {
                            throw new InvalidTransactionalOperationException("Key can not be null");
                        }
                        Remove(operation.Key);
                        break;
                    case TransactionalOperation.Clear:
                        Clear();
                        break;
                }
            }
        
            _pendingOperations.Clear();
        }
    }

    public void Rollback()
    {
        lock (_locker)
        {
            _pendingOperations.Clear();
        }
    }
}