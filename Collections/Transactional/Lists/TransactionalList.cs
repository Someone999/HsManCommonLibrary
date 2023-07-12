using HsManCommonLibrary.Exceptions;

namespace HsManCommonLibrary.Collections.Transactional.Lists;

public class TransactionalList<T> : List<T>
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
                    case TransactionalOperation.AddRange:
                        AddRange(operation.ValueCollection  ?? throw new InvalidTransactionalOperationException("Collection can not be null"));
                        break;
                    case TransactionalOperation.Remove:
                        Remove(operation.Value ?? throw new InvalidTransactionalOperationException("Value can not be null"));
                        break;
                    case TransactionalOperation.RemoveRange:
                        var valRemoveRange = operation.OperationRange;
                        if (valRemoveRange == null)
                        {
                            throw new InvalidTransactionalOperationException("OperationRange can not be null");
                        }

                        var realValRemoveRange = valRemoveRange.Value;
                        int countRemoveRange = realValRemoveRange.End - realValRemoveRange.Start;
                        int count = realValRemoveRange.End - realValRemoveRange.Start;
                        RemoveRange(realValRemoveRange.Start, count);
                        break;
                    case TransactionalOperation.Insert:
                        var valInsert = operation.InsertIndex;
                        if (valInsert == null || operation.Value == null)
                        {
                            throw new InvalidTransactionalOperationException("OperationRange and Value can not be null");
                        }

                        var realValInsert = valInsert.Value;
                        Insert(realValInsert, operation.Value);
                        break;
                    case TransactionalOperation.InsertRange:
                        var valInsertRange = operation.InsertIndex;
                        var collectionInsertRange = operation.ValueCollection;
                        if (valInsertRange == null || collectionInsertRange == null)
                        {
                            throw new InvalidTransactionalOperationException("OperationRange and Collection can not be null");
                        }

                        var realValInsertRange = valInsertRange.Value;
                        InsertRange(realValInsertRange, collectionInsertRange);
                        break;
                    case TransactionalOperation.Reverse:
                        Reverse();
                        break;
                    case TransactionalOperation.Sort:
                        switch (operation.CompareType)  
                        {
                            case CompareFlag.Comparer:
                                Sort(operation.Comparer);
                                break;
                            case CompareFlag.Comparison:
                                if (operation.Comparison == null)
                                {
                                    throw new InvalidTransactionalOperationException("Comparison can not be null");
                                }
                                Sort(operation.Comparison);
                                break;
                            case CompareFlag.ComparisonWithIndexRange:
                                var valComparisonWithIndexRange = operation.OperationRange;
                                if (valComparisonWithIndexRange == null)
                                {
                                    throw new InvalidTransactionalOperationException("OperationRange can not be null");
                                }

                                var realValComparisonWithIndexRange = valComparisonWithIndexRange.Value;
                                int countComparisonWithIndexRange = realValComparisonWithIndexRange.End - realValComparisonWithIndexRange.Start;
                                Sort(realValComparisonWithIndexRange.Start, countComparisonWithIndexRange, operation.Comparer);
                                break;
                            case CompareFlag.Default:
                                Sort();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        break;
                    case TransactionalOperation.Clear:
                        Clear();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
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