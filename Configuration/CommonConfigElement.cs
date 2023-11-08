using HsManCommonLibrary.Locks;
using HsManCommonLibrary.NestedValues;
using HsManCommonLibrary.NestedValues.NestedValueAdapters;
using HsManCommonLibrary.NestedValues.NestedValueConverters;
using HsManCommonLibrary.NestedValues.NestedValueDeserializer;
using HsManCommonLibrary.NestedValues.SaveStrategies;
using HsManCommonLibrary.ValueHolders;
using YamlDotNet.Serialization;

namespace HsManCommonLibrary.Configuration;

public class CommonConfigElement : INestedValueStore
{
    private readonly LockManager _lockManager = new LockManager();
    private readonly INestedValueStore _config;

    public CommonConfigElement(INestedValueStore nestedValueStore)
    {
        _config = nestedValueStore;
    }
    
    public CommonConfigElement(Dictionary<string, object> dictionary)
    {
        _config = new DictionaryNestedValueStoreAdapter().ToNestedValue(dictionary);
    }
    
    public CommonConfigElement(object obj)
    {
        _config = new DictionaryNestedValueStoreAdapter().ToNestedValue(obj);
    }
    
    public virtual object? GetValue()
    {
        lock (_lockManager.AcquireLockObject("ReadWriteLock"))
        {
            return _config.GetValue();
        }
    }

    public virtual T? GetValueAs<T>()
    {
        lock (_lockManager.AcquireLockObject("ReadWriteLock"))
        {
            if (typeof(T) != typeof(Dictionary<string, object>))
            {
                throw new InvalidCastException();
            }

            return (T?)_config;
        }
    }

    protected INestedValueStore? GetValue(string key)
    {
        lock (_lockManager.AcquireLockObject("ReadWriteLock"))
        {
            return _config[key];
        }
    }
    
    public virtual void SetValue(string key, INestedValueStore? val)
    {
        lock (_lockManager.AcquireLockObject("ReadWriteLock"))
        {
            var nestedVal = _config;
            if (nestedVal == null)
            {
                throw new InvalidOperationException();
            }
            
            nestedVal[key] = new CommonNestedValueStore(val);
        }
    }

    public virtual INestedValueStore? this[string key]
    {
        get => GetValue(key);
        set => SetValue(key, value);
    }

    public virtual object Convert(Type type)
    {
        lock (_lockManager.AcquireLockObject("ReadWriteLock"))
        {
            return _config;
        }
    }

    public virtual T? Convert<T>()
    {
        if (typeof(T) != typeof(Dictionary<string, INestedValueStore>))
        {
            throw new InvalidCastException();
        }

        lock (_lockManager.AcquireLockObject("ReadWriteLock"))
        {
            return (T?)_config;
        }
    }

    public virtual object ConvertWith(INestedValueStoreConverter converter)
    {
        throw new InvalidCastException();
    }

    public virtual T? ConvertWith<T>(INestedValueStoreConverter<T> converter)
    {
        if (typeof(T) != typeof(Dictionary<string, INestedValueStore>))
        {
            throw new InvalidCastException();
        }

        lock (_lockManager.AcquireLockObject("ReadWriteLock"))
        {
            return (T?)_config;
        }
    }

    public virtual bool IsNull(string key)
    {
        return false;
    }

    public virtual Dictionary<string, object?> ToDictionary()
    {
        lock (_lockManager.AcquireLockObject("ReadWriteLock"))
        {
            return ExpendInternal(_config.GetValue() as Dictionary<string, INestedValueStore> ??
                                  throw new InvalidCastException());
        }
    }

    protected virtual Dictionary<string, object?> ExpendInternal(Dictionary<string, INestedValueStore> dict)
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

    public void Persistence(string path, INestedValueStoreSaveStrategy saveStrategy)
    {
        saveStrategy.Save(this, path);
    }
    
    public Task PersistenceAsync(string path, INestedValueStoreSaveStrategy saveStrategy)
    {
        return saveStrategy.SaveAsync(this, path);
    }
   
    
    public T Deserialize<T>(INestedValueStoreDeserializer<T> storeDeserializer)
    {
        return storeDeserializer.Deserialize(this);
    }

    public ValueHolder<T> GetAsValueHolder<T>()
    {
        return new ValueHolder<T>(GetValueAs<T>());
    }

    public ValueHolder<INestedValueStore> GetMemberAsValueHolder(string memberName)
    {
        lock (_lockManager.AcquireLockObject("ReadWriteLock"))
        {
            return new ValueHolder<INestedValueStore>(_config[memberName]);
        }
    }

    public ValueHolder<T> GetMemberValueAsValueHolder<T>(string memberName)
    {
        lock (_lockManager.AcquireLockObject("ReadWriteLock"))
        {
            return new ValueHolder<T>((T?)_config[memberName]?.GetValue());
        }
    }

    public bool TryGetValue<T>(out T? value)
    {
        lock (_lockManager.AcquireLockObject("ReadWriteLock"))
        {
            value = (T)(object)_config;
            return true;
        }
    }

    public bool TryGetMember(string name, out INestedValueStore? value)
    {
        lock (_lockManager.AcquireLockObject("ReadWriteLock"))
        {
            var val = GetValue(name);
            if (val != null)
            {
                value = val;
                return true;
            }

            value = null;
            return false;
        }
    }

    public bool TryGetMemberValue<T>(string name, out T? value)
    {
        lock (_lockManager.AcquireLockObject("ReadWriteLock"))
        {
            var val = GetValue(name)?.GetValue();
            if (val == null)
            {
                value = default;
                return false;
            }

            value = (T)val;
            return true;
        }
    }
    
}