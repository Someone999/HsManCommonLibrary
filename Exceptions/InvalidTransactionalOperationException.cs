namespace HsManCommonLibrary.Exceptions;

public class InvalidTransactionalOperationException : HsManException
{
    public InvalidTransactionalOperationException(string message) : base(message)
    {
    }
}