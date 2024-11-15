namespace HsManCommonLibrary.Exceptions;

public class RecoverableException : HsManException
{
    public RecoverableException()
    {
    }

    public RecoverableException(string? message) : base(message)
    {
    }

    public RecoverableException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}