using System.Collections.Concurrent;
using HsManCommonLibrary.Collections.Transactional;
using HsManCommonLibrary.Collections.Transactional.Dictionaries;
using HsManCommonLibrary.Exceptions;

namespace HsManCommonLibrary.Collections.ConcurrentCollections;

public class TransactionalConcurrentDictionary<TKey, TValue> : ConcurrentDictionary<TKey, TValue> where TKey : notnull
{
    private ConcurrentBag<TransactionalDictionaryOperation<TKey, TValue>> _pendingOperations =
        new ConcurrentBag<TransactionalDictionaryOperation<TKey, TValue>>();

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
    }

    public void Rollback()
    {
        lock (_locker)
        {
            _pendingOperations = new ConcurrentBag<TransactionalDictionaryOperation<TKey, TValue>>();
        }
    }
}