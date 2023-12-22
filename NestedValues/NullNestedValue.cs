using HsManCommonLibrary.NestedValues.NestedValueConverters;
using HsManCommonLibrary.NestedValues.NestedValueDeserializer;
using HsManCommonLibrary.ValueHolders;

namespace HsManCommonLibrary.NestedValues;

public class NullNestedValue : INestedValueStore
{
    private NullNestedValue()
    {
    }

    public override bool Equals(object? obj) => obj is null or NullNestedValue;

    public override int GetHashCode() => 0;

    public override string ToString() => "";

    public static NullNestedValue Value { get; } = new NullNestedValue();
    
    public object? GetValue()
    {
        return null;
    }

    public T? GetValueAs<T>()
    {
        return default;
    }

    public void SetValue(string key, INestedValueStore? val)
    {
        throw new NotSupportedException("Can set value on a NullNestedObject");
    }

    public INestedValueStore? this[string key]
    {
        get => null;
        set
        {
        }
    }

    public object? Convert(Type type)
    {
        return null;
    }

    public T? Convert<T>()
    {
        return default;
    }

    public object? ConvertWith(INestedValueStoreConverter converter)
    {
        return null;
    }

    public T? ConvertWith<T>(INestedValueStoreConverter<T> converter)
    {
        return default;
    }

    public bool IsNull(string key)
    {
        return true;
    }

    public T Deserialize<T>(INestedValueStoreDeserializer<T> storeDeserializer)
    {
        throw new NotSupportedException();
    }

    public ValueHolder<T> GetAsValueHolder<T>()
    {
        return new ValueHolder<T>(default);
    }

    public ValueHolder<INestedValueStore> GetMemberAsValueHolder(string memberName)
    {
        return new ValueHolder<INestedValueStore>(default);
    }

    public ValueHolder<T> GetMemberValueAsValueHolder<T>(string memberName)
    {
        return new ValueHolder<T>(default);
    }

    public bool TryGetValue<T>(out T? value)
    {
        value = default;
        return false;
    }

    public bool TryGetMember(string name, out INestedValueStore? value)
    {
        value = default;
        return false;
    }

    public bool TryGetMemberValue<T>(string name, out T? value)
    {
        value = default;
        return false;
    }
}