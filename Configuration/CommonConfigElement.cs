using System.Text;
using HsManCommonLibrary.NestedValues;
using HsManCommonLibrary.NestedValues.NestedValueAdapters;
using HsManCommonLibrary.NestedValues.NestedValueConverters;
using HsManCommonLibrary.NestedValues.NestedValueDeserializer;
using HsManCommonLibrary.NestedValues.SaveStrategies;
using HsManCommonLibrary.ValueHolders;

namespace HsManCommonLibrary.Configuration;

public class CommonConfigElement : INestedValueStore
{
    private readonly INestedValueStore _config;

    public CommonConfigElement(INestedValueStore nestedValueStore)
    {
        _config = nestedValueStore;
    }

    public CommonConfigElement(Dictionary<string, object?> dictionary)
    {
        _config = new DictionaryNestedValueStoreAdapter().ToNestedValue(dictionary);
    }

    public CommonConfigElement(object obj)
    {
        _config = new DictionaryNestedValueStoreAdapter().ToNestedValue(obj as Dictionary<string, object?>);
    }

    public virtual object? GetValue()
    {
        return _config.GetValue();
    }

    public virtual T? GetValueAs<T>()
    {
        if (typeof(T) != typeof(Dictionary<string, object>))
        {
            throw new InvalidCastException();
        }

        return (T?)_config;
    }

    protected INestedValueStore? GetValue(string key)
    {
        return _config[key];
    }

    public virtual void SetValue(string key, INestedValueStore? val)
    {
        var nestedVal = _config;
        if (nestedVal == null)
        {
            throw new InvalidOperationException();
        }

        nestedVal[key] = new CommonNestedValueStore(val);
    }

    public virtual INestedValueStore? this[string key]
    {
        get => GetValue(key);
        set => SetValue(key, value);
    }

    public virtual object Convert(Type type)
    {
        return _config;
    }

    public virtual T? Convert<T>()
    {
        if (typeof(T) != typeof(Dictionary<string, INestedValueStore>))
        {
            throw new InvalidCastException();
        }

        return (T?)_config;
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


        return (T?)_config;
    }

    public virtual bool IsNull(string key)
    {
        return false;
    }

    public virtual Dictionary<string, object?> ToDictionary()
    {
        return ExpendInternal(_config.GetValue() as Dictionary<string, INestedValueStore> ??
                             throw new InvalidCastException());
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
                result.Add(pair.Key, Equals(nestedVal.GetValue(), NullNestedValue.Value) ? null : nestedVal.GetValue());
            }
        }

        return result;
    }

    public void Save(Stream stream, INestedValueStoreSaveStrategy saveStrategy, Encoding? encoding = null)
    {
        if (!saveStrategy.Validate(this))
        {
            throw new Exception("The nested value store is not valid for this save strategy");
        }

        saveStrategy.Save(this, stream, encoding);
    }

    public async Task SaveAsync(Stream stream, INestedValueStoreSaveStrategy saveStrategy, Encoding? encoding = null)
    {
        if (!saveStrategy.Validate(this))
        {
            throw new Exception("The nested value store is not valid for this save strategy");
        }


        await saveStrategy.SaveAsync(this, stream, encoding);
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
        return new ValueHolder<INestedValueStore>(_config[memberName]);
    }

    public ValueHolder<T> GetMemberValueAsValueHolder<T>(string memberName)
    {
        return new ValueHolder<T>((T?)_config[memberName]?.GetValue());
    }

    public bool TryGetValue<T>(out T? value)
    {
        value = (T)_config;
        return true;
    }

    public bool TryGetMember(string name, out INestedValueStore? value)
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

    public bool TryGetMemberValue<T>(string name, out T? value)
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
