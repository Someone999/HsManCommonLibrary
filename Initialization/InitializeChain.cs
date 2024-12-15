namespace HsManCommonLibrary.Initialization;

public class InitializeChain : IInitializeChain
{
    public List<IInitializer> Initializers { get; } = new();
    public void Initialize(IInitializer initializer, IInitializationManager initializationManager)
    {
        initializer.Initialize(initializationManager);
    }
    
    public void InitializeAll(IInitializationManager initializationManager)
    {
        foreach (var initializer in Initializers)
        {
            initializationManager.AddInitializationType(initializer.GetType());
        }
        
        InitializeStarted?.Invoke(this, EventArgs.Empty);
        
        foreach (var initializer in Initializers)
        {
            initializer.Initialize(initializationManager);
            if (initializationManager.ShouldAbort)
            {
                break;
            }
        }

        
        if (initializationManager.State.HasFlag(InitializeState.Failed))
        {
            InitializeFailed?.Invoke(this, EventArgs.Empty);
        }

        if (initializationManager.State.HasFlag(InitializeState.Completed))
        {
            InitializeSucceeded?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler? InitializeStarted;
    public event EventHandler? InitializeFailed;
    public event EventHandler? InitializeSucceeded;
}