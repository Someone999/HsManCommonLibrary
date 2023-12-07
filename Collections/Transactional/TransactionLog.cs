using HsManCommonLibrary.Collections.Transactional.Transactions;

namespace HsManCommonLibrary.Collections.Transactional;

public class TransactionLog<TCollection>
{
    public TransactionLog(ITransactionOperation<TCollection> operation, object? oldValue, object? newValue, OperationStatus status)
    {
        Operation = operation;
        OldValue = oldValue;
        NewValue = newValue;
        Status = status;
    }

    public DateTime OperationTime { get; } = DateTime.Now;
    public ITransactionOperation<TCollection> Operation { get; }
    public object? OldValue { get; set; }
    public object? NewValue { get; set; }
    public OperationStatus Status { get; set; }

    public void MarkOperationSuccess() => Status = OperationStatus.Success;
    public void MarkOperationFailed() => Status = OperationStatus.Failed;
    public void SetOperationStatus(OperationStatus status) => Status = status;
    public bool IsSuccess() => Status == OperationStatus.Success;
}