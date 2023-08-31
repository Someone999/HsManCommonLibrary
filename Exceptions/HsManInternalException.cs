namespace HsManCommonLibrary.Exceptions;

public class HsManInternalException : Exception
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