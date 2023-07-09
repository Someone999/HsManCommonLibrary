namespace CommonLibrary.Exceptions;

public abstract class PluginException : Exception
{
    protected PluginException()
    {
    }

    protected PluginException(string message) : base(message)
    {
    }

    protected PluginException(string message, Exception innerException) : base(message, innerException)
    {
    }
}