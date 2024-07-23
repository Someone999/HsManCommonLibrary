using System.Collections.Concurrent;

namespace HsManCommonLibrary.ValueHolders;

public class ReadonlyValueHolder<T> : IReadonlyValueHolder<T>
{
    public ReadonlyValueHolder(T? value)
    {
        Value = value;
        _initialized = Value != null;
    }

    public ReadonlyValueHolder(T? value, T? defaultValue)
    {
        Value = value;
        DefaultValue = defaultValue;
        _initialized = Value != null && !Value.Equals(defaultValue);
    }

    private readonly bool _initialized;

    public bool TryGetValueAs<TVal>(out TVal? value)
    {
        if (!IsInitialized())
        {
            value = default;
            return false;
        }
        
        if (typeof(TVal) == typeof(T))
        {
            value = (TVal?)(object?)Value;
            return true;
        }
        
        if (!typeof(IConvertible).IsAssignableFrom(typeof(TVal)) && !typeof(TVal).IsAssignableFrom(typeof(T)))
        {
            value = default;
            return false;
        }

        try
        {
            value = GetValueAs<TVal>();
            return true;
        }
        catch (Exception)
        {
            value = default;
            return false;
        }
    }

    object? IValueHolder.DefaultValue => DefaultValue;

    public T? Value { get; }
    public void BindValue(T value)
    {
        throw new InvalidOperationException("This bindable can only initialize in constructor.");
    }

    public T? DefaultValue { get; }

    object? IValueHolder.Value => Value;

    private ConcurrentDictionary<Type, object> _convertCache = new ConcurrentDictionary<Type, object>();
    public TVal GetValueAs<TVal>()
    {
        if (_convertCache.TryGetValue(typeof(TVal), out var val))
        {
            return (TVal)val;
        }
        
        if (Value == null)
        {
            throw new InvalidOperationException();
        }
        
        if (typeof(TVal) == typeof(T))
        {
            return (TVal)(object)Value;
        }

        var converted = Convert.ChangeType(Value, typeof(TVal));
        _convertCache.TryAdd(typeof(TVal), converted);
        return (TVal) converted;
    }

    public bool IsInitialized()
    {
        return _initialized;
    }

    public bool HasValue => _initialized && Value != null;
}