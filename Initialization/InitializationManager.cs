using HsManCommonLibrary.Timing;

namespace HsManCommonLibrary.Initialization;

public class InitializationManager : IInitializationManager
{
    public InitializationManager(InitializeContext context, IClock? clock = null)
    {
        Context = context;
        Clock = clock ?? new StopwatchClock();
        TimeManager = new InitializationTimeManager(Clock);
    }

    public void AddInitializationType(Type initializationType)
    {
        InitializationTypes.Add(initializationType);
        _initializationStates[initializationType] = InitializeState.None;
    }

    public void RemoveInitializationType(Type initializationType)
    {
        InitializationTypes.Remove(initializationType);
        _initializationStates.Remove(initializationType);
    }

    public bool ShouldAbort { get; private set; }
    private HashSet<Type> InitializationTypes { get; } = new HashSet<Type>();
    public InitializeContext Context { get; }
    public InitializeState State { get; private set; } = InitializeState.None;
    private readonly object _locker = new();
    private readonly Dictionary<Type, InitializeState> _initializationStates = new();
    public void ReportCompleted<T>(IInitializer initializer)
    {
        lock (_locker)
        {
            if (!_initializationStates.TryGetValue(typeof(T), out _))
            {
                throw new InvalidOperationException($"The type {typeof(T)} is not in the initialization manager");
            }
            
            _initializationStates[typeof(T)] = InitializeState.Completed;
            CompletedReported?.Invoke(this, new CompletedReportedEventArgs(initializer));
        }
       
    }

    public void ReportError<T>(IInitializer initializer, Exception? exception = null, string? message = null)
    {
        lock (_locker)
        {
            if (!_initializationStates.TryGetValue(typeof(T), out _))
            {
                throw new InvalidOperationException($"The type {typeof(T)} is not in the initialization manager");
            }
            
            _initializationStates[typeof(T)] = InitializeState.Completed;
            ErrorReported?.Invoke(this, new ErrorReportedEventArgs(initializer, exception, message));
        }
    }

    public void ReportFailed<T>(IInitializer initializer, string? message)
    {
        lock (_locker)
        {
            _initializationStates[typeof(T)] = InitializeState.Failed;
            State = InitializeState.Failed;
            FailedReported?.Invoke(this, new FailedReportedEventArgs(initializer, message));
        }
    }

    public void Abort()
    {
        lock (_locker)
        {
            ShouldAbort = true;
        }
    }

    public bool IsLoaded<T>() => _initializationStates[typeof(T)] == InitializeState.Completed;

    public InitializeState GetInitializeState<T>()
    {
        if (ShouldAbort)
        {
            return InitializeState.Aborted;
        }
        
        if (_initializationStates.Values.Any(s => s == InitializeState.Failed))
        {
            return InitializeState.Failed;
        }
        
        if (_initializationStates.Values.Any(s => s == InitializeState.Aborted))
        {
            return InitializeState.Aborted;
        }

        return _initializationStates.Values.All(s => s == InitializeState.Completed) 
            ? InitializeState.Completed 
            : InitializeState.Initializing;
    }

    public event EventHandler<CompletedReportedEventArgs>? CompletedReported;
    public event EventHandler<FailedReportedEventArgs>? FailedReported;
    public event EventHandler<ErrorReportedEventArgs>? ErrorReported;
    public IInitializationTimeManager TimeManager { get; }
    public IClock Clock { get; }
}