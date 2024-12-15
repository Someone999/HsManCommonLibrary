namespace HsManCommonLibrary.Initialization;

public class FailedReportedEventArgs : ReportedEventArgs
{
    public FailedReportedEventArgs(IInitializer initializer, string? message) : base(initializer)
    {
        Message = message;
    }

    public string? Message { get; set; }
}