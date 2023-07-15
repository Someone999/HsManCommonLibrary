namespace HsManCommonLibrary.NestedValues.NestedValueConverters;

public interface INestedValueStoreConverter
{
    object Convert(INestedValueStore nestValueStore);
}

public interface INestedValueStoreConverter<out T> : INestedValueStoreConverter
{
    new T Convert(INestedValueStore nestedValueStore);
}