namespace HsManCommonLibrary.Initialization;

public class InitializationTimeRecord : IInitializationTimeRecord
{
    public InitializationTimeRecord(Type componentType, TimeSpan initTime)
    {
        ComponentType = componentType;
        InitTime = initTime;
    }

    public Type ComponentType { get; }
    public TimeSpan InitTime { get; }
}