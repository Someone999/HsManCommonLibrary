using HsManCommonLibrary.Locks;
using HsManCommonLibrary.NestedValues.NestedValueAdapters;
using HsManCommonLibrary.NestedValues.NestedValueConverters;

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
            return _innerVal switch
            {
                Dictionary<string, INestedValueStore> dictionary => dictionary[key],
                Dictionary<string, object> objDict => new CommonNestedValueStore(objDict[key]),
                _ => NestedValueAdapterManager.GetAdapterByAdaptableType(_innerVal.GetType())
                    .ToConfigElement(_innerVal)[key]
            };
        }
    }
        
    public object GetValue()
    {
        return _innerVal;
    }

    public void SetValue(string key, INestedValueStore val)
    {
        lock (_lockManager.AcquireLockObject("SetValue"))
        {
            if (_innerVal is Dictionary<string, INestedValueStore> dictionary)
            {
                dictionary[key] = val;
            }

            throw new InvalidOperationException();
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
        return Equals(_innerVal, NullConfigValue.Value);
    }
}