using System.Text;

namespace HsManCommonLibrary.NestedValues.SaveStrategies;

public class IniNestedValueStoreSaveStrategy : INestedValueStoreSaveStrategy
{
    public void Initialize()
    {
    }

    
    
    public bool Validate(INestedValueStore nestedValueStore)
    {
        var storedVal = nestedValueStore.GetValue();
        if (storedVal is not Dictionary<string, INestedValueStore> dictionary)
        {
            return false;
        }

        var notSection = dictionary.Any(pair =>
            pair.Value.GetValue() is not Dictionary<string, INestedValueStore>);

        if (notSection)
        {
            return false;
        }
        
        foreach (var pair in dictionary)
        {
            if (pair.Value.GetValue() is not Dictionary<string, INestedValueStore>)
            {
                return false;
            }
        }

        return true;
    }

    public void Log()
    {
    }

    public void Save(INestedValueStore nestedValueStore, Stream stream, Encoding? encoding = null)
    {
        var storedVal = nestedValueStore.GetValue();
        if (storedVal is not Dictionary<string, INestedValueStore> dictionary)
        {
            return;
        }

        StringBuilder configBuilder = new StringBuilder();
        foreach (var pair in dictionary)
        {
            configBuilder.AppendLine($"[{pair.Key}]");
            if (pair.Value.GetValue() is not Dictionary<string, INestedValueStore> dictionary1)
            {
                continue;
            }
            
            
            
            foreach (var pair1 in dictionary1)
            {
                var val = pair1.Value.GetValue();
                if (Equals(val, NullNestedValue.Value))
                {
                    val = "null";
                }
                
                configBuilder.AppendLine($"{pair1.Key} = {val}");
            }
        }
        
        encoding ??= Encoding.UTF8;
        var bts = encoding.GetBytes(configBuilder.ToString());
        stream.Write(bts, 0, bts.Length);
    }

    public Task SaveAsync(INestedValueStore nestedValueStore, Stream stream, Encoding? encoding = null)
    {
        return Task.Run(() => Save(nestedValueStore, stream, encoding));
    }
}