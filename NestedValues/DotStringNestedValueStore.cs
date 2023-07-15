using HsManCommonLibrary.NestedValues.NestedValueConverters;

namespace HsManCommonLibrary.NestedValues;

public class DotStringNestedValueStore : INestedValueStore
{
    private INestedValueStore _nestedValue = new CommonNestedValueStore(new Dictionary<string, INestedValueStore>());
    public void Add(string dotString)
    {
        string[] levels = dotString.Split('.');
        if (levels.Any(string.IsNullOrEmpty))
        {
            return;
        }

        var currentLevelDict = _nestedValue.GetValueAs<Dictionary<string, INestedValueStore>>();
        Stack<INestedValueStore> stack = new Stack<INestedValueStore>();
        stack.Push(_nestedValue);
        foreach (var level in levels)
        {
            var trimedLevel = level.Trim();
            if (currentLevelDict.TryGetValue(trimedLevel, out var value) && !level.Contains('='))
            {
                stack.Push(value);
                currentLevelDict = value.GetValueAs<Dictionary<string, INestedValueStore>>();
            }
            else if(!level.Contains('='))
            {
                Dictionary<string, INestedValueStore> dict = new Dictionary<string, INestedValueStore>();
                INestedValueStore nestedValueStore = new CommonNestedValueStore(dict);
                if (!stack.Any())
                {
                    throw new KeyNotFoundException();
                }

                stack.Peek().GetValueAs<Dictionary<string, INestedValueStore>>().Add(trimedLevel, nestedValueStore);
                stack.Push(nestedValueStore);
                currentLevelDict = dict;
            }
            else
            {
                int eqIdx = trimedLevel.IndexOf('=');
                string propertyName = level.Substring(0, eqIdx).Trim();
                string propertyVal = level.Substring(eqIdx + 1).TrimStart();
                stack.Peek().GetValueAs<Dictionary<string, INestedValueStore>>().Add(propertyName, 
                    new CommonNestedValueStore(propertyVal));
            }
           
        }
    }
    
    
    public object GetValue()
    {
        return _nestedValue;
    }

    public T GetValueAs<T>()
    {
        if (typeof(T) != typeof(Dictionary<string, INestedValueStore>))
        {
            throw new InvalidCastException();
        }

        return (T)(object)_nestedValue.GetValueAs<Dictionary<string, INestedValueStore>>();
    }

    public void SetValue(string key, INestedValueStore val)
    {
        _nestedValue[key] = val;
    }

    public INestedValueStore this[string key]
    {
        get => _nestedValue[key];
        set => SetValue(key, value);
    }

    public object Convert(Type type)
    {
        if (type != typeof(Dictionary<string, INestedValueStore>))
        {
            throw new InvalidCastException();
        }

        return _nestedValue.GetValueAs<Dictionary<string, INestedValueStore>>();
    }

    public T Convert<T>() => (T)Convert(typeof(T));
   

    public object ConvertWith(INestedValueStoreConverter converter)
    {
        return converter.Convert(this);
    }

    public T ConvertWith<T>(INestedValueStoreConverter<T> converter)
    {
        return converter.Convert(this);
    }

    public bool IsNull(string key)
    {
        return _nestedValue[key].GetValue().Equals(NullObject.Value);
    }
}