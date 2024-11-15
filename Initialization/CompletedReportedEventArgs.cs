namespace HsManCommonLibrary.Initialization;

public class CompletedReportedEventArgs : ReportedEventArgs
{
    public CompletedReportedEventArgs(IInitializer initializer) : base(initializer)
    {
    }
}