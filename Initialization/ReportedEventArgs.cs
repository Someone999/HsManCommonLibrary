namespace HsManCommonLibrary.Initialization;

public abstract class ReportedEventArgs
{
    public ReportedEventArgs(IInitializer initializer)
    {
        Initializer = initializer;
    }

    public IInitializer Initializer { get; set; }
}