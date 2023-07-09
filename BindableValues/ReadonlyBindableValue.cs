namespace CommonLibrary.BindableValues;

public class ReadonlyBindableValue<T> : IReadonlyBindableValue<T>
{
    public ReadonlyBindableValue(T? value)
    {
        Value = value;
        _initialized = Value != null;
    }

    public ReadonlyBindableValue(T? value, T? defaultValue)
    {
        Value = value;
        DefaultValue = defaultValue;
        _initialized = Value != null && !Value.Equals(defaultValue);
    }

    private bool _initialized;
    
    object? IBindableValue.DefaultValue => DefaultValue;

    public T? Value { get; }
    public void BindValue(T value)
    {
        throw new InvalidOperationException("This bindable can only initialize in constructor.");
    }

    public T? DefaultValue { get; }

    object? IBindableValue.Value => Value;

    public TVal GetValueAs<TVal>()
    {
        if (Value == null)
        {
            throw new InvalidOperationException();
        }
        
        if (typeof(TVal) == typeof(T))
        {
            return (TVal)(object)Value;
        }

        return (TVal)Convert.ChangeType(Value, typeof(TVal));
    }

    public bool IsInitialized()
    {
        return _initialized;
    }
}