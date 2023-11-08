using HsManCommonLibrary.Configuration;
using HsManCommonLibrary.Configuration.Attributes;
using HsManCommonLibrary.Configuration.Utils;

namespace HsManCommonLibrary;

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