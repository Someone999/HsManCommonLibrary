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

    public bool ShouldAbort { get; private set; }
    public HashSet<Type> InitializationTypes { get; } = new HashSet<Type>();
    public InitializeContext Context { get; }
    public InitializeState State { get; private set; } = InitializeState.None;
    private readonly object _locker = new();
    private readonly Dictionary<Type, InitializeState> _initializationStates = new();
    public void ReportCompleted<T>()
    {
        lock (_locker)
        {
            if (!_initializationStates.TryGetValue(typeof(T), out _))
            {
                throw new InvalidOperationException($"The type {typeof(T)} is not in the initialization manager");
            }
            
            _initializationStates[typeof(T)] = InitializeState.Completed;
        }
       
    }

    public void ReportError<T>(Exception exception, string message)
    {
        lock (_locker)
        {
            if (!_initializationStates.TryGetValue(typeof(T), out _))
            {
                throw new InvalidOperationException($"The type {typeof(T)} is not in the initialization manager");
            }
            
            _initializationStates[typeof(T)] = InitializeState.Completed;
        }
    }

    public void ReportFailed<T>()
    {
        lock (_locker)
        {
            _initializationStates[typeof(T)] = InitializeState.Failed;
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

    public InitializeState GetInitializeStateState<T>()
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

    public IInitializationTimeManager TimeManager { get; }
    public IClock Clock { get; }
}