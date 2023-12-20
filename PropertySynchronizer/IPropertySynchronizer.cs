namespace HsManCommonLibrary.PropertySynchronizer;

public interface IPropertySynchronizer
{
    SynchronizeResult Synchronize(object sourceObj, object destinationObj);
}

public interface IPropertySynchronizer<in TSource> : IPropertySynchronizer
{
    SynchronizeResult Synchronize(TSource sourceObj, object destinationObj);
}

public interface IPropertySynchronizer<in TSource, in TDestination> : IPropertySynchronizer
{
    SynchronizeResult Synchronize(TSource sourceObj, TDestination destinationObj);
}