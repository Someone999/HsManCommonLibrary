using HsManCommonLibrary.NestedValues;

namespace HsManCommonLibrary.Configuration;

public interface IConfigurationRegistryCenter
{
    void RegisterConfig(string name, INestedValueStore nestedValueStore);
    INestedValueStore? ReplaceConfig(string name, INestedValueStore nestedValueStore);
    bool UnregisterConfig(string name);
    INestedValueStore this[string key] { get; }
}