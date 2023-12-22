using HsManCommonLibrary.Exceptions;

namespace HsManCommonLibrary.Utils;

public static class ExceptionUtils
{
    public static void ThrowWhen<TException>(bool condition, string? message, Exception? innerException = null)
    {
        if (!condition)
        {
            return;
        }

        var e = (Exception?) Activator.CreateInstance(typeof(TException), message, innerException);
        if (e == null)
        {
            throw new HsManInternalException("Failed to initialize exception object.");
        }

        throw e;
    }
}