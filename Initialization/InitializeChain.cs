namespace HsManCommonLibrary.Initialization;

public class InitializeChain : IInitializeChain
{
    public List<IInitializer> Initializers { get; } = new();
    public InitializeState Initialize(IInitializer initializer, IInitializationManager initializationManager)
    {
        return initializer.Initialize(initializationManager);
    }
    
    public void InitializeAll(IInitializationManager initializationManager)
    {
        foreach (var initializer in Initializers)
        {
            initializationManager.InitializationTypes.Add(initializer.GetType());
        }
        
        InitializeStarted?.Invoke(this, EventArgs.Empty);
        
        foreach (var initializer in Initializers)
        {
            var state = initializer.Initialize(initializationManager);
            if (initializationManager.ShouldAbort)
            {
                ComponentInitializeAborted?.Invoke(initializer, initializationManager);
            }

            switch (state)
            {
                case InitializeState.Completed:
                    ComponentInitializeSucceeded?.Invoke(initializer, initializationManager);
                    break;
                case InitializeState.Failed:
                    ComponentInitializeFailed?.Invoke(initializer, initializationManager);
                    break;
                case InitializeState.Aborted:
                    ComponentInitializeAborted?.Invoke(initializer, initializationManager);
                    break;
            }
        }
    }

    public event EventHandler? InitializeStarted;

    public event EventHandler? InitializeFailed;
    public event EventHandler? InitializeSucceeded;
    public event EventHandler? InitializeCompleted;
    public event Action<IInitializer, IInitializationManager>? ComponentInitializeSucceeded;
    public event Action<IInitializer, IInitializationManager>? ComponentInitializeFailed;
    public event Action<IInitializer, IInitializationManager>? ComponentInitializeAborted;
}