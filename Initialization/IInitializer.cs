namespace HsManCommonLibrary.Initialization;

public interface IInitializer
{
    public InitializeState Initialize(IInitializationManager initializationManager);
}