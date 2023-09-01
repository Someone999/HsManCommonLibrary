namespace HsManCommonLibrary.ValueHolders;

public class ValueHolder<T> : IValueHolder<T>
{
    private bool _initialized;
    public ValueHolder(T? defaultValue, T? value)
    {
        DefaultValue = defaultValue;
        _value = value;
        _initialized = value is not null;
    }

    public ValueHolder(T? value)
    {
        _value = value;
        _initialized = value is not null;
    }
        
    public ValueHolder()
    {
    }
        

    public event Action<ValueChangedEventArgs<T>>? OnValueChanged;
    public T? DefaultValue { get; }

    private T? _value;
    public T? Value
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

    public T? GetValueOrDefault(T? defVal) => Value ?? defVal;
    
    public T GetValueOrDefaultNonNull(T defVal) => Value ?? defVal;

    object? IValueHolder.DefaultValue => DefaultValue;
    object? IValueHolder.Value => Value;
}