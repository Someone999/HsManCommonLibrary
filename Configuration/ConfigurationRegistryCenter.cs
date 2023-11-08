using System.Collections.Concurrent;
using HsManCommonLibrary.NestedValues;

namespace HsManCommonLibrary.Configuration;

public class ConfigurationRegistryCenter : IConfigurationRegistryCenter
{
    private readonly ConcurrentDictionary<string, INestedValueStore> _registeredConfig = new();

    public void RegisterConfig(string name, INestedValueStore nestedValueStore)
    {
        _registeredConfig.TryAdd(name, nestedValueStore);
    }

    public INestedValueStore? ReplaceConfig(string name, INestedValueStore nestedValueStore)
    {
        _registeredConfig.TryGetValue(name, out var oldVal);
        _registeredConfig[name] = nestedValueStore;
        return oldVal;
    }

    public bool UnregisterConfig(string name)
    {
        return _registeredConfig.TryRemove(name, out _);
    }

    public INestedValueStore this[string key] => _registeredConfig[key];
}