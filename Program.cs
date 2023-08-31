using System.Text;
using HsManCommonLibrary.Configuration;
using HsManCommonLibrary.Configuration.Attributes;
using HsManCommonLibrary.Configuration.Utils;
using HsManCommonLibrary.NestedValues;
using HsManCommonLibrary.NestedValues.NestedValueAdapters;
using HsManCommonLibrary.Reflections;
using HsManCommonLibrary.Timers;

namespace HsManCommonLibrary;

static class Global
{
    public static IConfigurationRegistryCenter ConfigurationRegistryCenter { get; } = new ConfigurationRegistryCenter();
}

class AConfig : ISyncableConfig
{
    public IConfigurationRegistryCenter RegistryCenter => Global.ConfigurationRegistryCenter;
    [ConfigSyncItem("main.test.var0")]
    public int TestInt1 { get; set; } = 0;
    
    [ConfigSyncItem("main.test.var1")]
    public int TestInt2 { get; set; } = 1;
    public void Save()
    {
        ConfigAssigner.AssignFrom(this);
    }
}

class Program
{
   
    static void Main(string[] args)
    {
        Global.ConfigurationRegistryCenter.RegisterConfig("main", new JsonConfigElement("test.json"));
        AConfig aConfig = new AConfig();
        ConfigAssigner.AssignTo(aConfig, null);
        aConfig.TestInt1 = 256;
        aConfig.TestInt2 = 512;
        aConfig.Save();
    }
}