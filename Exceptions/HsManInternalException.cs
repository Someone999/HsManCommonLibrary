namespace HsManCommonLibrary.Exceptions;

public class HsManInternalException : HsManException
{
    public HsManInternalException()
    {
    }

    public HsManInternalException(string? message) : base(message)
    {
    }

    public HsManInternalException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}