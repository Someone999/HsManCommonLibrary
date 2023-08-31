using HsManCommonLibrary.Locks;
using HsManCommonLibrary.NestedValues.NestedValueAdapters;
using HsManCommonLibrary.NestedValues.NestedValueConverters;
using HsManCommonLibrary.NestedValues.NestedValueDeserializer;

namespace HsManCommonLibrary.NestedValues;

public class CommonNestedValueStore : INestedValueStore
{
    private readonly object _innerVal;
    private LockManager _lockManager = new LockManager();

    public CommonNestedValueStore(object innerVal)
    {
        _innerVal = innerVal;
    }

    private INestedValueStore GetConfigElement(string key)
    {
        lock (_lockManager.AcquireLockObject("GetConfigElement"))
        {
            switch (_innerVal)
            {
                case Dictionary<string, INestedValueStore> dictionary:
                    return dictionary[key];
                case Dictionary<string, object> objDict:
                    return new CommonNestedValueStore(objDict[key]);
                default:
                        var r = NestedValueAdapterManager.GetAdapterByAdaptableType(_innerVal.GetType())?
                            .ToNestedValue(_innerVal)[key];

                    return new CommonNestedValueStore(r ?? _innerVal);
            }
        }
    }
        
    public object GetValue()
    {
        return _innerVal;
    }

    public T GetValueAs<T>()
    {
        return (T)_innerVal;
    }

    public void SetValue(string key, INestedValueStore val)
    {
        lock (_lockManager.AcquireLockObject("SetValue"))
        {
            
            var tmpValue = _innerVal;
            var adapter = NestedValueAdapterManager.GetAdapterByAdaptableType(_innerVal.GetType());
            if (adapter != null)
            {
                tmpValue = adapter.ToNestedValue(_innerVal).GetValue();
            }
            


            if (tmpValue is not Dictionary<string, INestedValueStore> dictionary)
            {
                throw new InvalidOperationException();
            }
            
            dictionary[key] = val;
        }
    }

    public INestedValueStore this[string key]
    {
        get => GetConfigElement(key);
        set => SetValue(key, value);
    }

    public object Convert(Type type)
    {
        lock (_lockManager.AcquireLockObject("Convert"))
        {
            return _innerVal is IConvertible 
                ? System.Convert.ChangeType(_innerVal, type) 
                : throw new InvalidCastException();
        }
    }

    public T Convert<T>() => (T) Convert(typeof(T));
       

    public object ConvertWith(INestedValueStoreConverter converter)
    {
        lock (_lockManager.AcquireLockObject("ConvertWith"))
        {
            return converter.Convert(this);
        }
    }

    public T ConvertWith<T>(INestedValueStoreConverter<T> converter)
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
    
}