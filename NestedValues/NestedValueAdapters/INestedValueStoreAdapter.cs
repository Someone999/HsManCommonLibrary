namespace HsManCommonLibrary.NestedValues.NestedValueAdapters;

public interface INestedValueStoreAdapter
{
    INestedValueStore ToNestedValue(object? obj);
    bool CanConvert(Type t);
}