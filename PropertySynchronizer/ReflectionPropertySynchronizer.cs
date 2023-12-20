using System.Reflection;

namespace HsManCommonLibrary.PropertySynchronizer;

public class ReflectionPropertySynchronizer : IPropertySynchronizer
{
    public SynchronizeResult Synchronize(object sourceObj, object destinationObj)
    {
        Dictionary<string, PropertyInfo> sourceProperties =
            sourceObj.GetType().GetProperties().ToDictionary(p => p.Name);

        PropertyInfo[] destProperties = destinationObj.GetType().GetProperties();
        List<FailedProperty> failedProperties = new List<FailedProperty>();
        int processedProperty = 0;
        foreach (var property in destProperties)
        {
            try
            {
                if (!sourceProperties.TryGetValue(property.Name, out var propertyInfo))
                {
                    continue;
                }

                processedProperty++;
                property.SetValue(destinationObj, propertyInfo.GetValue(sourceObj));
            }
            catch (Exception e)
            {
                processedProperty++;
                failedProperties.Add(new FailedProperty(property, e));
            }
        }

        SynchronizeState synchronizeState = failedProperties.Count ==
                                            processedProperty ? SynchronizeState.Failed :
            failedProperties.Count == 0 ? SynchronizeState.Success : SynchronizeState.PartialSuccess;

        return new SynchronizeResult(synchronizeState, failedProperties.ToArray());
    }
}