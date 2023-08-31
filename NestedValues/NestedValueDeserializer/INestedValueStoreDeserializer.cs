namespace HsManCommonLibrary.NestedValues.NestedValueDeserializer;

public interface INestedValueStoreDeserializer
{
    bool CanConvert(Type t);
    object Deserialize(INestedValueStore nestedValueStore);
}


public interface INestedValueStoreDeserializer<out T> : INestedValueStoreDeserializer
{
    new T Deserialize(INestedValueStore nestedValueStore);
}

