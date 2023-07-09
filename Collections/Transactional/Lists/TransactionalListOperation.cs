namespace CommonLibrary.Collections.Transactional.Lists;

public class TransactionalListOperation<T>
{
    public TransactionalOperation Operation { get; set; }
    public T? Value { get; set; }
    public IEnumerable<T>? ValueCollection { get; set; }
    public IndexRange? OperationRange { get; set; }
    public int? InsertIndex { get; set; }
    public IComparer<T>? Comparer { get; set; }
    public Comparison<T>? Comparison { get; set; }
    public CompareFlag? CompareType { get; set; }
        

    public static TransactionalListOperation<T> CreateAdd(T value)
    {
        return new TransactionalListOperation<T>()
        {
            Operation = TransactionalOperation.Add,
            Value = value
        };
    }
        
    public static TransactionalListOperation<T> CreateAddRange(IEnumerable<T> valueCollection)
    {
        return new TransactionalListOperation<T>()
        {
            Operation = TransactionalOperation.AddRange,
            ValueCollection = valueCollection
        };
    }
        
    public static TransactionalListOperation<T> CreateRemove(T value)
    {
        return new TransactionalListOperation<T>()
        {
            Operation = TransactionalOperation.Remove,
            Value = value
        };
    }
        
    public static TransactionalListOperation<T> CreateRemoveRange(IndexRange indexRange)
    {
        return new TransactionalListOperation<T>()
        {
            Operation = TransactionalOperation.RemoveRange,
            OperationRange = indexRange
        };
    }
        
    public static TransactionalListOperation<T> CreateRemoveRange(int start, int end)
    {
        return new TransactionalListOperation<T>()
        {
            Operation = TransactionalOperation.RemoveRange,
            OperationRange = new IndexRange(start, end)
        };
    }
        
    public static TransactionalListOperation<T> CreateInsert(T value, int index)
    {
        return new TransactionalListOperation<T>()
        {
            Operation = TransactionalOperation.Insert,
            Value = value,
            InsertIndex = index
        };
    }
        
    public static TransactionalListOperation<T> CreateInsertRange(T[] valueCollection, int index)
    {
        return new TransactionalListOperation<T>()
        {
            Operation = TransactionalOperation.Insert,
            ValueCollection = valueCollection,
            InsertIndex = index
        };
    }

    public TransactionalListOperation<T> CreateSort(IComparer<T> comparer)
    {
        return new TransactionalListOperation<T>()
        {
            Comparer = comparer,
            CompareType = CompareFlag.Comparer
        };
    }
        
    public TransactionalListOperation<T> CreateSort(IComparer<T> comparer, IndexRange range)
    {
        return new TransactionalListOperation<T>()
        {
            Comparer = comparer,
            OperationRange = range,
            CompareType = CompareFlag.ComparisonWithIndexRange
        };
    }
        
    public TransactionalListOperation<T> CreateSort(Comparison<T> comparison)
    {
        return new TransactionalListOperation<T>()
        {
            Comparison = comparison,
            CompareType = CompareFlag.Comparison
        };
    }

    public static TransactionalListOperation<T> CreateClear()
    {
        return new TransactionalListOperation<T>()
        {
            Operation = TransactionalOperation.Clear,
        };
    }

}