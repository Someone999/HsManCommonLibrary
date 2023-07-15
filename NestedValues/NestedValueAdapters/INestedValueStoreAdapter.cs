namespace HsManCommonLibrary.NestedValues.NestedValueAdapters;

public interface INestedValueStoreAdapter
{
    INestedValueStore ToConfigElement(object? obj);
    bool CanConvert(Type t);
}