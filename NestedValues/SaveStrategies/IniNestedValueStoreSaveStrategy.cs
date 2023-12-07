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

    public void Save(INestedValueStore nestedValueStore, string path)
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
                if (Equals(val, NullObject.Value))
                {
                    val = "null";
                }
                
                configBuilder.AppendLine($"{pair1.Key} = {val}");
            }
        }
        
        File.WriteAllText(path, configBuilder.ToString());
    }

    public Task SaveAsync(INestedValueStore nestedValueStore, string path)
    {
        return Task.Run(() => Save(nestedValueStore, path));
    }
}