using HsManCommonLibrary.Locks;
using HsManCommonLibrary.NestedValues;
using HsManCommonLibrary.NestedValues.NestedValueConverters;
using HsManCommonLibrary.NestedValues.NestedValueDeserializer;
using HsManCommonLibrary.ValueHolders;

namespace HsManCommonLibrary.Configuration;

public class DotStringNestedValueStore : INestedValueStore
{
    private readonly INestedValueStore _nestedValue = new CommonNestedValueStore(new Dictionary<string, INestedValueStore>());
    private readonly LockManager _lockManager = new LockManager();
    public void Add(string dotString)
    {
        string[] levels = dotString.Split('.');
        if (levels.Any(string.IsNullOrEmpty))
        {
            return;
        }

        var currentLevelDict = _nestedValue.GetValueAs<Dictionary<string, INestedValueStore>>();
        Stack<INestedValueStore?> stack = new Stack<INestedValueStore?>();
        stack.Push(_nestedValue);
        foreach (var level in levels)
        {
            var trimedLevel = level.Trim();
            if (currentLevelDict == null)
            {
                throw new InvalidOperationException();
            }

            if (currentLevelDict.TryGetValue(trimedLevel, out var value) && !level.Contains('='))
            {
                stack.Push(value);
                currentLevelDict = value.GetValueAs<Dictionary<string, INestedValueStore>>();
            }
            else if (!level.Contains('='))
            {
                Dictionary<string, INestedValueStore> dict = new Dictionary<string, INestedValueStore>();
                INestedValueStore nestedValueStore = new CommonNestedValueStore(dict);
                if (!stack.Any())
                {
                    throw new KeyNotFoundException();
                }

                var currentLevel = stack.Peek();
                if (currentLevel == null)
                {
                    throw new NullReferenceException();
                }

                var storedVal = currentLevel.GetValueAs<Dictionary<string, INestedValueStore>>();
                if (storedVal == null)
                {
                    throw new NullReferenceException();
                }

                storedVal.Add(trimedLevel, nestedValueStore);
                stack.Push(nestedValueStore);
                currentLevelDict = dict;
            }
            else
            {
                int eqIdx = trimedLevel.IndexOf('=');
                string propertyName = level.Substring(0, eqIdx).Trim();
                string propertyVal = level.Substring(eqIdx + 1).TrimStart();
                var storedVal1 = stack.Peek()?.GetValueAs<Dictionary<string, INestedValueStore>>();
                if (storedVal1 == null)
                {
                    throw new NullReferenceException();
                }
                storedVal1.Add(propertyName, new CommonNestedValueStore(propertyVal));
            }
        }
    }


    public object GetValue()
    {
        lock (_lockManager.AcquireLockObject("ReadWriteLock"))
        {
            return _nestedValue;
        }
    }

    public T? GetValueAs<T>()
    {
        if (typeof(T) != typeof(Dictionary<string, INestedValueStore>))
        {
            throw new InvalidCastException();
        }

        lock (_lockManager.AcquireLockObject("ReadWriteLock"))
        {
            return (T?)(object?)_nestedValue.GetValueAs<Dictionary<string, INestedValueStore>>();
        }
    }

    public void SetValue(string key, INestedValueStore? val)
    {
        lock (_lockManager.AcquireLockObject("ReadWriteLock"))
        {
            _nestedValue[key] = val;
        }
    }

    private INestedValueStore? GetValue(string key)
    {
        lock (_lockManager.AcquireLockObject("ReadWriteLock"))
        {
            _nestedValue.GetValueAs<Dictionary<string, INestedValueStore>>()
                !.TryGetValue(key, out var r);

            return r;
        }
    }
    public INestedValueStore? this[string key]
    {
        get => GetValue(key);
        set => SetValue(key, value);
    }

    public object? Convert(Type type)
    {
        if (type != typeof(Dictionary<string, INestedValueStore>))
        {
            throw new InvalidCastException();
        }

        lock (_lockManager.AcquireLockObject("ReadWriteLock"))
        {
            return _nestedValue.GetValueAs<Dictionary<string, INestedValueStore>>();
        }
    }

    public T? Convert<T>() => (T?)Convert(typeof(T));


    public object? ConvertWith(INestedValueStoreConverter converter)
    {
        return converter.Convert(this);
    }

    public T? ConvertWith<T>(INestedValueStoreConverter<T> converter)
    {
        return converter.Convert(this);
    }

    public bool IsNull(string key)
    {
        lock (_lockManager.AcquireLockObject("ReadWriteLock"))
        {
            var val = _nestedValue[key];
            var storedVal = val?.GetValue();
            if (val == null || storedVal == null)
            {
                throw new KeyNotFoundException();
            }

            return storedVal.Equals(NullObject.Value);
        }
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
            return new ValueHolder<INestedValueStore>(this[memberName]);
        }
    }

    public ValueHolder<T> GetMemberValueAsValueHolder<T>(string memberName)
    {
        lock (_lockManager.AcquireLockObject("ReadWriteLock"))
        {
            return new ValueHolder<T>((T?)this[memberName]?.GetValue());
        }
    }

    public bool TryGetValue<T>(out T? value)
    {
        value = (T)_nestedValue;
        return true;
    }

    public bool TryGetMember(string name, out INestedValueStore? value)
    {
        var val = this[name];
        if (val == null)
        {
            value = null;
            return false;
        }

        value =  val;
        return true;
    }

    public bool TryGetMemberValue<T>(string name, out T? value)
    {
        var val = this[name]?.GetValue();
        if (val == null)
        {
            value = default;
            return false;
        }

        value = (T) val;
        return true;
    }
}