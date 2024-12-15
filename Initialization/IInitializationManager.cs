using HsManCommonLibrary.Timing;

namespace HsManCommonLibrary.Initialization;

public interface IInitializationManager
{
    void AddInitializationType(Type type);
    void RemoveInitializationType(Type type);
    bool ShouldAbort { get; }
    InitializeContext Context { get; }
    InitializeState State { get; }
    void ReportCompleted<T>(IInitializer initializer);
    void ReportError<T>(IInitializer initializer, Exception? exception = null, string? message = null);
    void ReportFailed<T>(IInitializer initializer, string? message = null);
    void Abort();
    bool IsLoaded<T>();
    event EventHandler<CompletedReportedEventArgs>? CompletedReported;
    event EventHandler<FailedReportedEventArgs>? FailedReported;
    event EventHandler<ErrorReportedEventArgs>? ErrorReported;
    
    InitializeState GetInitializeState<T>();
    IInitializationTimeManager TimeManager { get; }
    IClock Clock { get; }
}