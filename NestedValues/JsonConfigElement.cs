using HsManCommonLibrary.Locks;
using HsManCommonLibrary.NestedValues.NestedValueConverters;
using HsManCommonLibrary.ValueHolders;
using Newtonsoft.Json;

namespace HsManCommonLibrary.NestedValues;

public class JsonConfigElement : INestedValueStore
{
    private ValueHolder<Dictionary<string, object>> _config;
    private LockManager _lockManager = new LockManager();
        
    public JsonConfigElement(string jsonFile)
    {
        lock (_lockManager.AcquireLockObject(".ctor"))
        {
            var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(jsonFile));
            if (dict == null)
            {
                throw new InvalidOperationException();
            }

            _config = new ValueHolder<Dictionary<string, object>>(dict);

        }
    }
        
    public object GetValue()
    {
        return _config.Value;
    }

    public T GetValueAs<T>()
    {
        if (typeof(T) != typeof(Dictionary<string, object>))
        {
            throw new InvalidCastException();
        }

        return (T)(object)_config.Value;
    }

    private INestedValueStore GetConfigElement(string key)
    {
        lock (_lockManager.AcquireLockObject("GetConfigElement"))
        {
            var val = _config.Value[key];
            return val switch
            {
                INestedValueStore configElement => configElement[key],
                { } => new CommonNestedValueStore(val),
                _ => throw new InvalidCastException()
            };
        }
    }
    
    public void SetValue(string key, INestedValueStore val)
    {
        lock (_lockManager.AcquireLockObject("SetValue"))
        {
            _config.Value[key] = new CommonNestedValueStore(val);
        }
    }

    public INestedValueStore this[string key]
    {
        get => GetConfigElement(key);
        set => SetValue(key, value);
    }

    public object? Convert(Type type)
    {
        return _config;
    }

    public T Convert<T>()
    {
        if (typeof(T) != typeof(Dictionary<string, INestedValueStore>))
        {
            throw new InvalidCastException();
        }

        return (T)(object)_config.Value;
    }

    public object ConvertWith(INestedValueStoreConverter converter)
    {
        throw new InvalidCastException();
    }

    public T ConvertWith<T>(INestedValueStoreConverter<T> converter)
    {
        if (typeof(T) != typeof(Dictionary<string, INestedValueStore>))
        {
            throw new InvalidCastException();
        }

        return (T)(object)_config.Value;
    }

    public bool IsNull(string key)
    {
        return false;
    }
}