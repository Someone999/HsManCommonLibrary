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
    }

    public INestedValueStore? this[string key]
    {
        get => null;
        set
        {
        }
    }

    public bool IsNull(string key)
    {
        return true;
    }
}