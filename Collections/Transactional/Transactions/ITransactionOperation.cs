namespace HsManCommonLibrary.Collections.Transactional.Transactions;

public interface ITransactionOperation<TCollection>
{
    IEnumerable<TransactionLog<TCollection>> CreateLogSet();
    bool Apply(TCollection collection);
    bool Undo(TCollection collection, TransactionLog<TCollection> transactionLog);
    object? GetOldValue();
    bool IsCompleted();
}