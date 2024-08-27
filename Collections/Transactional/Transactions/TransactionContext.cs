

namespace HsManCommonLibrary.Collections.Transactional.Transactions;

public class TransactionContext<TCollection>
{
    private readonly ReaderWriterLockSlim _readerWriterLockSlim = new ReaderWriterLockSlim();
    public TransactionStatus TransactionStatus { get; private set; } = TransactionStatus.Started;

    private readonly TCollection _originalCollection;
    private readonly List<TransactionLog<TCollection>> _executedTransactions = new List<TransactionLog<TCollection>>();
    private readonly List<ITransactionOperation<TCollection>> _operations = new List<ITransactionOperation<TCollection>>();

    internal TransactionContext(TCollection originalCollection)
    {
        _originalCollection = originalCollection;
    }

    public void AddOperation(ITransactionOperation<TCollection> operation)
    {
        _operations.Add(operation);
    }

    public void Commit()
    {
        _readerWriterLockSlim.EnterWriteLock();
        foreach (var operation in _operations)
        {
            if (operation.Apply(_originalCollection))
            {
                _executedTransactions.AddRange(operation.CreateLogSet());
            }
            else
            {
                TransactionStatus = TransactionStatus.Aborted;
                return;
            }
        }
        
        _executedTransactions.Clear();
        _operations.Clear();
        TransactionStatus = TransactionStatus.Committed;
        _readerWriterLockSlim.ExitWriteLock();
    }

    public void Rollback()
    {
        _readerWriterLockSlim.EnterWriteLock();
        foreach (var transactionLog in _executedTransactions)
        {
            transactionLog.Operation.Undo(_originalCollection, transactionLog);
        }
        
        _readerWriterLockSlim.ExitWriteLock();
    }
}
