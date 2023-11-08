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
        var rootConfig = new Dictionary<string, object>();
        foreach (var line in lines)
        {
            if (line.StartsWith("#") || line.StartsWith(";") || string.IsNullOrEmpty(line))
            {
                rootConfig.Add($"#Comment{commentIdx++}", line);
                continue;
            }

            if (line.StartsWith("[") && line.EndsWith("]"))
            {
                appKey = line.Substring(1, line.Length - 1);
                if (rootConfig.ContainsKey(appKey))
                {
                    continue;
                }

                rootConfig.Add(appKey, new Dictionary<string, object>());
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
                if (rootConfig[appKey ?? throw new InvalidOperationException()] is not Dictionary<string, object> d0)
                {
                    throw new InvalidOperationException();
                }
                
                d0.Add(propertyName, valueStr);
            }
        }

        return new CommonConfigElement(rootConfig);
    }

    public bool CanConvert(Type t)
    {
        return false;
    }
}