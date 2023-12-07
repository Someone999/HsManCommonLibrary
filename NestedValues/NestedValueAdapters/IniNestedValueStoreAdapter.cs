using HsManCommonLibrary.Configuration;

namespace HsManCommonLibrary.NestedValues.NestedValueAdapters;

public class IniNestedValueStoreAdapter : INestedValueStoreAdapter
{
    private readonly string _configFile;
    public IniNestedValueStoreAdapter(string path)
    {
        _configFile = path;
    }
    
    public INestedValueStore ToNestedValue(object? obj = null)
    {
        string[] lines = File.ReadAllLines(_configFile);
        string? appKey = null;
        int commentIdx = 0;
        var rootConfig = new Dictionary<string, INestedValueStore>();
        foreach (var line in lines)
        {
            if (line.StartsWith("#") || line.StartsWith(";") || string.IsNullOrEmpty(line))
            {
                rootConfig.Add($"#Comment{commentIdx++}", new CommonNestedValueStore(line));
                continue;
            }

            if (line.StartsWith("[") && line.EndsWith("]"))
            {
                appKey = line.Substring(1, line.Length - 2);
                if (rootConfig.ContainsKey(appKey))
                {
                    continue;
                }

                rootConfig.Add(appKey, new CommonNestedValueStore(new Dictionary<string, INestedValueStore>()));
            }
            else
            {
                int colonIndex = line.IndexOf('=');
                if (colonIndex == -1)
                {
                    continue;
                }

                var propertyName = line.Substring(0, colonIndex).Trim();
                var valueStr = line.Substring(colonIndex + 1).Trim();
                if (rootConfig[appKey ?? throw new InvalidOperationException()].GetValue() is not Dictionary<string, INestedValueStore> d0)
                {
                    throw new InvalidOperationException();
                }
                
                d0.Add(propertyName, new CommonNestedValueStore(valueStr));
            }
        }

        return new CommonNestedValueStore(rootConfig);
    }

    public bool CanConvert(Type t)
    {
        return false;
    }
}