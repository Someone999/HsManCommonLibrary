namespace HsManCommonLibrary.Initialization;

public interface IInitializationManager
{
    void AddInitializationType(Type initializationType);
    void RemoveInitializationType(Type initializationType);
    bool ShouldAbort { get; }
    InitializeContext Context { get; }
    InitializeState State { get; }
    void ReportCompleted<T>(IInitializer initializer);
    void ReportError<T>(IInitializer initializer, Exception? exception = null, string? message = null);
    void ReportFailed<T>(IInitializer initializer, string? message = null);
    void Abort();
    bool IsLoaded<T>();
    InitializeState GetInitializeState<T>();
    event EventHandler<CompletedReportedEventArgs>? CompletedReported;
    event EventHandler<FailedReportedEventArgs>? FailedReported;
    event EventHandler<ErrorReportedEventArgs>? ErrorReported;
    
}