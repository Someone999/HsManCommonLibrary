using CommonLibrary.Exceptions;

namespace CommonLibrary.Collections.Transactional.Dictionaries;

public class TransactionalDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TKey: notnull
{
    private List<TransactionalDictionaryOperation<TKey, TValue>> _pendingOperations =
        new List<TransactionalDictionaryOperation<TKey, TValue>>();

    public void AddTransactionalOperation(TransactionalDictionaryOperation<TKey, TValue> operation)
    {
        _pendingOperations.Add(operation);
    }
        
    public void Commit()
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
    }

    public void Rollback()
    {
        _pendingOperations.Clear();
    }
}