using HsManCommonLibrary.NestedValues.NestedValueConverters;
using HsManCommonLibrary.NestedValues.NestedValueDeserializer;
using HsManCommonLibrary.ValueHolders;

namespace HsManCommonLibrary.NestedValues;

public interface INestedValueStore
{
    object? GetValue();
    T? GetValueAs<T>();
    void SetValue(string key, INestedValueStore? val);
    INestedValueStore? this[string key] { get; set; }
    object? Convert(Type type);
    T? Convert<T>();
    object? ConvertWith(INestedValueStoreConverter converter);
    T? ConvertWith<T>(INestedValueStoreConverter<T> converter);
    bool IsNull(string key);
    T Deserialize<T>(INestedValueStoreDeserializer<T> storeDeserializer);
    ValueHolder<T> GetAsValueHolder<T>();
    ValueHolder<INestedValueStore> GetMemberAsValueHolder(string memberName);
    ValueHolder<T> GetMemberValueAsValueHolder<T>(string memberName);
    bool TryGetValue<T>(out T? value);
    bool TryGetMember(string name, out INestedValueStore? value);
    bool TryGetMemberValue<T>(string name, out T? value);
}


