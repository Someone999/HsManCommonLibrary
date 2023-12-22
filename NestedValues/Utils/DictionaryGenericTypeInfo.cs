namespace HsManCommonLibrary.NestedValues.Utils;

public class DictionaryGenericTypeInfo
{
    public DictionaryGenericTypeInfo(Type keyType, Type valueType)
    {
        KeyType = keyType;
        ValueType = valueType;
    }

    public Type KeyType { get; }
    public Type ValueType { get; }
}