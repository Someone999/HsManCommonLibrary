using HsManCommonLibrary.Locks;
using HsManCommonLibrary.NestedValues.NestedValueAdapters;
using HsManCommonLibrary.NestedValues.NestedValueConverters;
using HsManCommonLibrary.NestedValues.NestedValueDeserializer;
using HsManCommonLibrary.ValueHolders;

namespace HsManCommonLibrary.NestedValues;

public class CommonNestedValueStore : INestedValueStore
{
    private readonly object? _innerVal;
    private LockManager _lockManager = new LockManager();

    public CommonNestedValueStore(object? innerVal)
    {
        _innerVal = innerVal;
    }

    private INestedValueStore? GetValue(string key)
    {
        lock (_lockManager.AcquireLockObject("GetConfigElement"))
        {
            switch (_innerVal)
            {
                case Dictionary<string, INestedValueStore> dictionary:
                    return !dictionary.ContainsKey(key) ? null : dictionary[key];
                case Dictionary<string, object> objDict:
                    return new CommonNestedValueStore(!objDict.ContainsKey(key) ? null : objDict[key]);
                default:
                    if (_innerVal == null)
                    {
                        return null;
                    }
                    
                    var r = NestedValueAdapterManager.GetAdapterByAdaptableType(_innerVal.GetType())?
                        .ToNestedValue(_innerVal)[key];

                    return new CommonNestedValueStore(r ?? _innerVal);
            }
        }
    }
        
    public object? GetValue()
    {
        return _innerVal;
    }

    public T? GetValueAs<T>()
    {
        if (_innerVal == null)
        {
            return default;
        }
        
        return (T)_innerVal;
    }

    public void SetValue(string key, INestedValueStore? val)
    {
        lock (_lockManager.AcquireLockObject("SetValue"))
        {
            
            var tmpValue = _innerVal;
            var objType = _innerVal?.GetType();
            if(objType == null)
            {
                return;
            }
            
            var adapter = NestedValueAdapterManager.GetAdapterByAdaptableType(objType);
            if (adapter != null)
            {
                tmpValue = adapter.ToNestedValue(_innerVal).GetValue();
            }
            


            if (tmpValue is not Dictionary<string, INestedValueStore> dictionary)
            {
                throw new InvalidOperationException();
            }

            dictionary[key] = val ?? throw new ArgumentNullException();
        }
    }

    public INestedValueStore? this[string key]
    {
        get => GetValue(key);
        set => SetValue(key, value);
    }

    public object? Convert(Type type)
    {
        lock (_lockManager.AcquireLockObject("Convert"))
        {
            return _innerVal is IConvertible 
                ? System.Convert.ChangeType(_innerVal, type) 
                : throw new InvalidCastException();
        }
    }

    public T? Convert<T>() => (T?) Convert(typeof(T));
       

    public object? ConvertWith(INestedValueStoreConverter converter)
    {
        lock (_lockManager.AcquireLockObject("ConvertWith"))
        {
            return converter.Convert(this);
        }
    }

    public T? ConvertWith<T>(INestedValueStoreConverter<T> converter)
    {
        lock (_lockManager.AcquireLockObject("ConvertWith<T>"))
        {
            return converter.Convert(this);
        }
        
    }

    public bool IsNull(string key)
    {
        return Equals(_innerVal, NullObject.Value);
    }

    public T Deserialize<T>(INestedValueStoreDeserializer<T> storeDeserializer)
    {
        return storeDeserializer.Deserialize(this);
    }

    public ValueHolder<T> GetAsValueHolder<T>()
    {
        ValueHolder<T> valueHolder = new ValueHolder<T>(GetValueAs<T>());
        return valueHolder;
    }

    public ValueHolder<T> GetMemberAsValueHolder<T>(string memberName)
    {
        return new ValueHolder<T>((T?)GetValue(memberName));
    }

    public ValueHolder<T> GetMemberValueAsValueHolder<T>(string memberName)
    {
        return new ValueHolder<T>((T?)GetValue(memberName)?.GetValue());
    }

    public bool TryGetValue<T>(out T? value)
    {
        if (_innerVal == null)
        {
            value = default;
            return false;
        }

        value = (T)_innerVal;
        return true;
    }

    public bool TryGetMember<T>(string name, out INestedValueStore? value)
    {
        var memberVal = GetValue(name);
        if (memberVal == null)
        {
            value = null;
            return false;
        }

        value = memberVal;
        return true;
    }

    public bool TryGetMemberValue<T>(string name, out T? value)
    {
        var val = GetValue(name);
        if (val == null)
        {
            value = default;
            return false;
        }

        value = (T) val;
        return true;
    }
}