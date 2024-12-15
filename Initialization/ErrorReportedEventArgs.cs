namespace HsManCommonLibrary.Initialization;

public class ErrorReportedEventArgs : ReportedEventArgs
{
    public ErrorReportedEventArgs(IInitializer initializer, Exception? exception, string? message) : base(initializer)
    {
        Exception = exception;
        Message = message;
    }

    public Exception? Exception { get; set; }
    public string? Message { get; set; }
}