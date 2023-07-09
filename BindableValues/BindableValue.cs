namespace CommonLibrary.BindableValues;

public class BindableValue<T> : IBindableValue<T>
{
    private bool _initialized;
    public BindableValue(T defaultValue, T value)
    {
        DefaultValue = defaultValue;
        _value = value;
    }

    public BindableValue(T value)
    {
        _value = value;
    }
        
    public BindableValue()
    {
    }
        

    public event Action<ValueChangedEventArgs<T>>? OnValueChanged;
    public T? DefaultValue { get; }

    private T? _value;
    public T Value
    {
        get
        {
            if (!_initialized || _value == null)
            {
                throw new InvalidOperationException("Value is not initialized.");
            }

            return _value;
        }
    }

    private readonly object _locker = new object();
    public void BindValue(T value)
    {
        lock (_locker)
        {
            if (EqualityUtils.Equals(value, _value))
            {
                return;
            }

            if (!_initialized)
            {
                _initialized = true;
            }
            
            OnValueChanged?.Invoke(new ValueChangedEventArgs<T>(_value, value));
            _value = value;
        }
    }

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

    public bool IsInitialized() => _initialized;
       

    object? IBindableValue.DefaultValue => DefaultValue;
    object? IBindableValue.Value => Value;
}