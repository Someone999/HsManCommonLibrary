namespace HsManCommonLibrary.PropertySynchronizer;

public class SynchronizeResult
{
    public SynchronizeResult(SynchronizeState synchronizeState, FailedProperty[] failedProperties)
    {
        SynchronizeState = synchronizeState;
        FailedProperties = failedProperties;
    }

    public SynchronizeState SynchronizeState { get; }
    public FailedProperty[] FailedProperties { get; }

    public bool HasError => FailedProperties.Length != 0 && SynchronizeState == SynchronizeState.Success;

    public Dictionary<string, FailedProperty> GetFailedPropertyAsDictionary()
    {
        return FailedProperties.ToDictionary(p => p.PropertyInfo.Name);
    }
}