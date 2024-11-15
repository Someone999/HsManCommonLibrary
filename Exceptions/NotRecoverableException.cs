namespace HsManCommonLibrary.Exceptions;

public class NotRecoverableException : HsManException
{
    public NotRecoverableException()
    {
    }

    public NotRecoverableException(string? message) : base(message)
    {
    }

    public NotRecoverableException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}