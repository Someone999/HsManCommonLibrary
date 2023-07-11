namespace HsManCommonLibrary.Exceptions;

public class InvalidTransactionalOperationException : PluginException
{
    public InvalidTransactionalOperationException(string message) : base(message)
    {
    }
}