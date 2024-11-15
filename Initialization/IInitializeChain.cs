namespace HsManCommonLibrary.Initialization;

public interface IInitializeChain
{
    void Initialize(IInitializer initializer, IInitializationManager initializationManager);
    void InitializeAll(IInitializationManager initializationManager);

    event EventHandler? InitializeStarted;
    event EventHandler? InitializeFailed;
    event EventHandler? InitializeSucceeded;
}