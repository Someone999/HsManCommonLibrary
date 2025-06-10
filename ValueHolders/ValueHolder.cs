using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using HsManCommonLibrary.Utils;

namespace HsManCommonLibrary.ValueHolders;

public class ValueHolder<T> : IValueHolder<T>
{
    private bool _initialized;
    private int _version = -1;
    private int? _lastVersion;
    private Type? _valueType;
    private Type? _lastValueType;
    public ValueHolder(T? defaultValue, T? value)
    {
        DefaultValue = defaultValue;
        _value = value;
        _initialized = value is not null;
        _valueType = value?.GetType() ?? typeof(T);
    }

    public ValueHolder(T? value = default)
    {
        _value = value;
        _initialized = value is not null;
        _valueType = value?.GetType() ?? typeof(T);
    }


    public event Action<ValueChangedEventArgs<T>>? ValueChanged;
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
    public void SetValue(T value)
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
            
            ValueChanged?.Invoke(new ValueChangedEventArgs<T>(_value, value));
            _value = value;
            _lastVersion = _version;
            _lastValueType = _valueType;
            _version++;
            _valueType = value?.GetType();
        }
    }


    private ConcurrentDictionary<Type, object> _convertCache = new ConcurrentDictionary<Type, object>();
    public TVal GetValueAs<TVal>()
    {
        var versionMatch = _lastVersion == _version && (_lastValueType == null || _lastValueType == _valueType);
        if (_convertCache.TryGetValue(typeof(TVal), out var val) && versionMatch)
        {
            return (TVal)val;
        }
        
        if (Value == null)
        {
            throw new InvalidOperationException();
        }

        _convertCache.TryRemove(typeof(TVal), out _);
        if (typeof(TVal) == typeof(T))
        {
            return (TVal)(object)Value;
        }

        var converted = Convert.ChangeType(Value, typeof(TVal));
        _convertCache.TryAdd(typeof(TVal), converted);
        return (TVal) converted;
    }

    public bool IsInitialized() => _initialized;
    public bool HasValue => _initialized && Value != null;
    
    public T? GetValueOrDefault(T? defVal) => Value ?? defVal;
    
    public T GetValueOrDefaultNotNull(T defVal) => Value ?? defVal;

    object? IValueHolder.DefaultValue => DefaultValue;
    object? IValueHolder.Value => Value;
}