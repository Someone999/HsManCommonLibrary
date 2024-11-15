using HsManCommonLibrary.NestedValues.NestedValueDeserializer;

namespace HsManCommonLibrary.NestedValues;

public interface IDeserializableNestedValueStore
{
    T Deserialize<T>(INestedValueStoreDeserializer<T> storeDeserializer);
}