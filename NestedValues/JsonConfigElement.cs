using HsManCommonLibrary.Locks;
using HsManCommonLibrary.NestedValues.NestedValueAdapters;
using HsManCommonLibrary.NestedValues.NestedValueConverters;
using HsManCommonLibrary.NestedValues.NestedValueDeserializer;
using HsManCommonLibrary.ValueHolders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HsManCommonLibrary.NestedValues;

public class JsonConfigElement : IPersistableNestedValueStore
{
    private readonly ValueHolder<INestedValueStore> _config;
    private readonly LockManager _lockManager = new LockManager();
        
    public JsonConfigElement(string jsonFile)
    {
        lock (_lockManager.AcquireLockObject(".ctor"))
        {
            var dict = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(jsonFile));
            JsonNestedValueStoreAdapter adapter = new JsonNestedValueStoreAdapter();
            var adapted = adapter.ToNestedValue(dict);
            if (adapted == null)
            {
                throw new InvalidOperationException();
            }

            _config = new ValueHolder<INestedValueStore>(adapted);

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
            return _config.Value[key];
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

    public Dictionary<string, object?> ToDictionary()
    {
        return ExpendInternal(_config.Value.GetValue() as Dictionary<string, INestedValueStore> ?? throw new InvalidCastException());
    }

    Dictionary<string, object?> ExpendInternal(Dictionary<string, INestedValueStore> dict)
    {
        Dictionary<string, object?> result = new Dictionary<string, object?>();
        foreach (var pair in dict)
        {
            var nestedVal = pair.Value;
            
            if (nestedVal.GetValue() is Dictionary<string, INestedValueStore> d)
            {
                result.Add(pair.Key, ExpendInternal(d));
            }
            else
            {
                result.Add(pair.Key, Equals(nestedVal.GetValue(), NullObject.Value) ? null : nestedVal.GetValue());
            }
        }

        return result;
    }
    public void Persistence(string path)
    {
        string json = JsonConvert.SerializeObject(ToDictionary(), Formatting.Indented);
        File.WriteAllText(path, json);
    }
    
    public T Deserialize<T>(INestedValueStoreDeserializer<T> storeDeserializer)
    {
        return storeDeserializer.Deserialize(this);
    }
}