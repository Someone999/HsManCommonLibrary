using System.Collections.Concurrent;
using CommonLibrary.Collections.Transactional;
using CommonLibrary.Collections.Transactional.Dictionaries;
using CommonLibrary.Exceptions;

namespace CommonLibrary.Collections.ConcurrentCollections;

public class TransactionalConcurrentDictionary<TKey, TValue> : ConcurrentDictionary<TKey, TValue> where TKey : notnull
{
    private ConcurrentBag<TransactionalDictionaryOperation<TKey, TValue>> _pendingOperations =
        new ConcurrentBag<TransactionalDictionaryOperation<TKey, TValue>>();

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
                    
                    TryAdd(operation.Key, operation.Value);
                    break;
                case TransactionalOperation.Remove:
                    if (operation.Key == null)
                    {
                        throw new InvalidTransactionalOperationException("Key can not be null");
                    }
                    TryRemove(operation.Key, out _);
                    break;
                case TransactionalOperation.Clear:
                    Clear();
                    break;
            }
        }
    }

    public void Rollback()
    {
        _pendingOperations = new ConcurrentBag<TransactionalDictionaryOperation<TKey, TValue>>();
    }
}