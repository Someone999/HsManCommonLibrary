namespace CommonLibrary.Exceptions;

public class PluginDomainLoadFailedException : PluginException
{
    public PluginDomainLoadFailedException() : this("Failed to load plugin domain")
    {
    }

    public PluginDomainLoadFailedException(string message) : base(message)
    {
    }

    public PluginDomainLoadFailedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}