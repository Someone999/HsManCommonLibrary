using HsManCommonLibrary.Timing;

namespace HsManCommonLibrary.Initialization;

public interface IInitializationManager
{
    HashSet<Type> InitializationTypes { get; }
    bool ShouldAbort { get; }
    InitializeContext Context { get; }
    InitializeState State { get; }
    void ReportCompleted<T>();
    void ReportError<T>(Exception exception, string message);
    void ReportFailed<T>();
    void Abort();
    bool IsLoaded<T>();
    InitializeState GetInitializeStateState<T>();
    IInitializationTimeManager TimeManager { get; }
    IClock Clock { get; }
}