namespace HsManCommonLibrary.Collections.Transactional.Transactions;

public class TransactionContextFactory<TCollection>
{
    public TransactionContext<TCollection> BeginTransaction(TCollection collection)
    {
        return new TransactionContext<TCollection>(collection);
    }
}