using HsManCommonLibrary.NestedValues.NestedValueConverters;

namespace HsManCommonLibrary.NestedValues;

public interface IConvertibleNestedValueStore
{
    object? Convert(Type type);
    T? Convert<T>();
    object? ConvertWith(INestedValueStoreConverter converter);
    T? ConvertWith<T>(INestedValueStoreConverter<T> converter);
}