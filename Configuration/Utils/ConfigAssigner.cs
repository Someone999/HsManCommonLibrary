using System.Reflection;
using HsManCommonLibrary.NestedValues;
using HsManCommonLibrary.NestedValues.Attributes;
using HsManCommonLibrary.NestedValues.NestedValueConverters;

namespace HsManCommonLibrary.Configuration.Utils;

public static class ConfigAssigner
{

    static ConfigPathPrefixProcessResult ProcessPrefix(Type syncableConfigType, string configName)
    {
        if (!configName.Contains("::"))
        {
            return new ConfigPathPrefixProcessResult(syncableConfigType, configName);
        }

        string[] nameParts = configName.Split(':', ':');
        return nameParts[0] switch
        {
            "base" => new ConfigPathPrefixProcessResult(GetBaseType(syncableConfigType), nameParts[2]),
            "this" => new ConfigPathPrefixProcessResult(syncableConfigType, nameParts[2]),
            _ => new ConfigPathPrefixProcessResult(syncableConfigType, configName)
        };
    }
        
    static Type GetBaseType(Type derivedType)
    {
        if (derivedType.BaseType == null)
        {
            throw new InvalidOperationException($"Type {derivedType} has no base type");
        }

        return derivedType.BaseType;
    }
    
    
    static object? GetMemberValue(object obj, MemberInfo memberInfo)
    {
        return memberInfo.MemberType switch
        {
            MemberTypes.Field => ((FieldInfo)memberInfo).GetValue(obj),
            MemberTypes.Method => ((MethodInfo)memberInfo).Invoke(obj, Array.Empty<object>()),
            MemberTypes.Property => ((PropertyInfo)memberInfo).GetValue(obj),
            _ => throw new NotSupportedException()
        };
    }
    
    static INestedValueStore RecursiveGetConfig(ISyncableConfig syncableConfig, string configPath)
    {
        string[] configPathLevels = configPath.Split('.');
        string configName = configPathLevels[0];
        INestedValueStore nestedValueStore = syncableConfig.RegistryCenter[configName];
        INestedValueStore currentNestedValueStore = nestedValueStore;
        foreach (var level in configPathLevels.Skip(1))
        {
            string tmpLevel = level;
            if (level.StartsWith("<") && level.EndsWith(">"))
            {
                string memberName = level.Substring(1, level.Length - 2);
                var searchType = syncableConfig.GetType();
                var result = ProcessPrefix(searchType, memberName);
                searchType = result.SearchType;
                memberName = result.MemberName;
                const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;
                var members = searchType.GetMember(memberName, bindingFlags);
                
                if (members.Length == 0 || members == null)
                {
                    throw new InvalidOperationException(
                        $"Member {memberName} is not in the type or it's a non-public member.");
                }

                var member = members[0];
                var memberValue = GetMemberValue(syncableConfig, member);
                if (memberValue == null)
                {
                    throw new ArgumentNullException();
                }
                
                tmpLevel = memberValue.ToString() ?? throw new ArgumentNullException();
            }
            
            currentNestedValueStore = currentNestedValueStore[tmpLevel] ?? throw new KeyNotFoundException();
        }

        return currentNestedValueStore;
    }
    
    static INestedValueStore RecursiveGetParentConfig(ISyncableConfig syncableConfig, string configPath
        , out string valueElementName)
    {
        string[] configPathLevels = configPath.Split('.');
        string configName = configPathLevels[0];
        int len = configPathLevels.Length - 2;
        INestedValueStore nestedValueStore = syncableConfig.RegistryCenter[configName];
        INestedValueStore currentNestedValueStore = nestedValueStore;
        foreach (var level in configPathLevels.Skip(1).Take(len))
        {
            string tmpLevel = level;
            if (level.StartsWith("<") && level.EndsWith(">"))
            {
                string memberName = level.Substring(1, level.Length - 2);
                var searchType = syncableConfig.GetType();
                var result = ProcessPrefix(searchType, memberName);
                searchType = result.SearchType;
                memberName = result.MemberName;
                
                
                const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;
                var members = searchType.GetMember(memberName, bindingFlags);
                
                if (members.Length == 0 || members == null)
                {
                    throw new InvalidOperationException(
                            $"Member {memberName} is not in the type or it's a non-public member.");
                }

                var member = members[0];
                var memberValue = GetMemberValue(syncableConfig, member);
                if (memberValue == null)
                {
                    throw new TargetException("Failed to get value of target member");
                }

                tmpLevel = memberValue.ToString() ?? throw new 
                    NullReferenceException("Failed to get value of target member");
            }
            
            currentNestedValueStore = currentNestedValueStore[tmpLevel] ?? throw new KeyNotFoundException();
        }

        valueElementName = configPathLevels.Last();
        return currentNestedValueStore;
    }
    
    public static void AssignTo(ISyncableConfig syncableConfig, 
        Dictionary<Type, Dictionary<string, object?>>? typeConverterConstructorArgs)
    {
        
        var syncableConfigType = syncableConfig.GetType();
        const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;
        var attrType = typeof(AutoAssignAttribute);
        var properties = 
            syncableConfigType.GetProperties(bindingFlags).Where(p => p.IsDefined(attrType)).ToArray();
        foreach (var property in properties)
        {
            var attr = property.GetCustomAttribute<AutoAssignAttribute>();
            if (attr == null)
            {
                continue;
            }

            var propertyValue = property.GetValue(syncableConfig);
            if (propertyValue is ISyncableConfig memberSyncableConfig && string.IsNullOrEmpty(attr.Path))
            {
                AssignTo(memberSyncableConfig, typeConverterConstructorArgs);
            }
            
            
            var currentCfg = RecursiveGetConfig(syncableConfig, attr.Path);
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
                var val = currentCfg.GetValue();
                if (Equals(val, NullNestedValue.Value))
                {
                    val = null;
                }
                
                var r = Convert.ChangeType(val, property.PropertyType);
                property.SetValue(syncableConfig, r);
            }
        }
    }

    public static void AssignFrom(ISyncableConfig syncableConfig)
    {
        var syncableConfigType = syncableConfig.GetType();
        const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;
        var attrType = typeof(AutoAssignAttribute);
        var properties = 
            syncableConfigType.GetProperties(bindingFlags).Where(p => p.IsDefined(attrType)).ToArray();
        foreach (var property in properties)
        {
            var attr = property.GetCustomAttribute<AutoAssignAttribute>();
            if (attr == null)
            {
                continue;
            }

            var propertyValue = property.GetValue(syncableConfig);
            if (propertyValue is ISyncableConfig memberSyncableConfig && string.IsNullOrEmpty(attr.Path))
            {
                AssignFrom(memberSyncableConfig);
            }
            
            
            var currentCfg = RecursiveGetParentConfig(syncableConfig, attr.Path, out var memberName);
            currentCfg.SetValue(memberName, new CommonNestedValueStore(property.GetValue(syncableConfig) ?? NullNestedValue.Value));
        }
    }
}