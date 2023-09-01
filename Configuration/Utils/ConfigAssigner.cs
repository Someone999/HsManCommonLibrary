using System.Globalization;
using System.Reflection;
using HsManCommonLibrary.Configuration.Attributes;
using HsManCommonLibrary.NestedValues;
using HsManCommonLibrary.NestedValues.NestedValueConverters;

namespace HsManCommonLibrary.Configuration.Utils;

public static class ConfigAssigner
{
    static INestedValueStore RecursiveGetConfig(IConfigurationRegistryCenter registryCenter, string configPath)
    {
        string[] configPathLevels = configPath.Split('.');
        string configName = configPathLevels[0];
        INestedValueStore nestedValueStore = registryCenter[configName];
        INestedValueStore currentNestedValueStore = nestedValueStore;
        foreach (var level in configPathLevels.Skip(1))
        {
            
            currentNestedValueStore = currentNestedValueStore[level] ?? throw new KeyNotFoundException();
        }

        return currentNestedValueStore;
    }
    
    static INestedValueStore RecursiveGetParentConfig(IConfigurationRegistryCenter registryCenter, string configPath
        , out string memberName)
    {
        string[] configPathLevels = configPath.Split('.');
        string configName = configPathLevels[0];
        int len = configPathLevels.Length - 2;
        INestedValueStore nestedValueStore = registryCenter[configName];
        INestedValueStore currentNestedValueStore = nestedValueStore;
        foreach (var level in configPathLevels.Skip(1).Take(len))
        {
            currentNestedValueStore = currentNestedValueStore[level] ?? throw new KeyNotFoundException();
        }

        memberName = configPathLevels.Last();
        return currentNestedValueStore;
    }
    
    public static void AssignTo(ISyncableConfig syncableConfig, 
        Dictionary<Type, Dictionary<string, object?>>? typeConverterConstructorArgs)
    {
        
        var syncableConfigType = syncableConfig.GetType();
        BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;
        var attrType = typeof(ConfigSyncItemAttribute);
        var properties = 
            syncableConfigType.GetProperties(bindingFlags).Where(p => p.IsDefined(attrType)).ToArray();
        foreach (var property in properties)
        {
            var attr = property.GetCustomAttribute<ConfigSyncItemAttribute>();
            if (attr == null)
            {
                continue;
            }

            var propertyValue = property.GetValue(syncableConfig);
            if (propertyValue is ISyncableConfig memberSyncableConfig && attr.ConfigPath == null)
            {
                AssignTo(memberSyncableConfig, typeConverterConstructorArgs);
            }

            if (attr.ConfigPath == null)
            {
                continue;
            }
            
            var currentCfg = RecursiveGetConfig(syncableConfig.RegistryCenter, attr.ConfigPath);
            if (attr.ConverterType != null)
            {
                var converterType = attr.ConverterType;
                object?[] args = Array.Empty<object>();
                if ( typeConverterConstructorArgs != null && 
                    typeConverterConstructorArgs.TryGetValue(converterType, out var arg))
                {
                    args = arg.Values.ToArray();
                }

                var converter = (INestedValueStoreConverter?) Activator.CreateInstance(converterType, args);
                
                var r = converter?.Convert(currentCfg);
                property.SetValue(syncableConfig, r);
            }
            else
            {
                var r = Convert.ChangeType(currentCfg.GetValue(), property.PropertyType);
                property.SetValue(syncableConfig, r);
            }
        }
    }

    public static void AssignFrom(ISyncableConfig syncableConfig)
    {
        var syncableConfigType = syncableConfig.GetType();
        BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;
        var attrType = typeof(ConfigSyncItemAttribute);
        var properties = 
            syncableConfigType.GetProperties(bindingFlags).Where(p => p.IsDefined(attrType)).ToArray();
        foreach (var property in properties)
        {
            var attr = property.GetCustomAttribute<ConfigSyncItemAttribute>();
            if (attr == null)
            {
                continue;
            }

            var propertyValue = property.GetValue(syncableConfig);
            if (propertyValue is ISyncableConfig memberSyncableConfig && attr.ConfigPath == null)
            {
                AssignFrom(memberSyncableConfig);
            }

            if (attr.ConfigPath == null)
            {
                continue;
            }
            
            var currentCfg = RecursiveGetParentConfig(syncableConfig.RegistryCenter, attr.ConfigPath, out var memberName);
            currentCfg.SetValue(memberName, new CommonNestedValueStore(property.GetValue(syncableConfig) ?? NullObject.Value));
        }
    }
}