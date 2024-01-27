using System.Runtime.Serialization;

namespace HsManCommonLibrary.Exceptions;

public class HsManException : Exception
{
    public HsManException()
    {
    }
    

    public HsManException(string? message) : base(message)
    {
    }

    public HsManException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}