namespace HsManCommonLibrary.Collections.Transactional.Transactions;

public enum TransactionStatus
{
    Committed,
    Rollback,
    Started,
    Aborted
}