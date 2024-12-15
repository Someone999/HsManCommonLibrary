namespace HsManCommonLibrary.Initialization;

public interface IInitializeChain
{
    InitializeState Initialize(IInitializer initializer, IInitializationManager initializationManager);
    void InitializeAll(IInitializationManager initializationManager);

    event EventHandler? InitializeStarted;
    event EventHandler? InitializeFailed;
    event EventHandler? InitializeSucceeded;
    event EventHandler? InitializeCompleted;
    
    event Action<IInitializer, IInitializationManager>? ComponentInitializeSucceeded;
    event Action<IInitializer, IInitializationManager>? ComponentInitializeFailed;
    event Action<IInitializer, IInitializationManager>? ComponentInitializeAborted;
}