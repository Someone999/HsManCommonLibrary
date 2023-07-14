using HsManCommonLibrary.ValueHolders;
using HsManCommonLibrary.ConfigElements.ConfigConverters;
using HsManCommonLibrary.Locks;
using Newtonsoft.Json;

namespace HsManCommonLibrary.ConfigElements;

public class JsonConfigElement : IConfigElement
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
        
    private IConfigElement GetConfigElement(string key)
    {
        lock (_lockManager.AcquireLockObject("GetConfigElement"))
        {
            var val = _config.Value[key];
            return val switch
            {
                IConfigElement configElement => configElement[key],
                { } => new CommonConfigElement(val),
                _ => throw new InvalidCastException()
            };
        }
    }
    
    public void SetValue(string key, IConfigElement val)
    {
        lock (_lockManager.AcquireLockObject("SetValue"))
        {
            _config.Value[key] = new CommonConfigElement(val);
        }
    }

    public IConfigElement this[string key]
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
        if (typeof(T) != typeof(Dictionary<string, IConfigElement>))
        {
            throw new InvalidCastException();
        }

        return (T)(object)_config.Value;
    }

    public object ConvertWith(IConfigConverter converter)
    {
        throw new InvalidCastException();
    }

    public T ConvertWith<T>(IConfigConverter<T> converter)
    {
        if (typeof(T) != typeof(Dictionary<string, IConfigElement>))
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