using System.Collections.Concurrent;
using HsManCommonLibrary.Collections.Transactional;
using HsManCommonLibrary.Collections.Transactional.Lists;
using HsManCommonLibrary.Exceptions;

namespace HsManCommonLibrary.Collections.ConcurrentCollections;

public class TransactionalConcurrentBag<T> : ConcurrentBag<T>
{
    private List<TransactionalListOperation<T>> _pendingOperations = new List<TransactionalListOperation<T>>();
    private readonly object _locker = new object();
    public void AddTransactionalOperation(TransactionalListOperation<T> operation)
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
                        Add(operation.Value ?? throw new InvalidTransactionalOperationException("Value can not be null"));
                        break;
                    default:
                        throw new InvalidTransactionalOperationException(
                            $"This collection doesn't support operation {operation.Operation}");
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