using HsManCommonLibrary.Configuration;
using HsManCommonLibrary.Configuration.Utils;
using HsManCommonLibrary.NestedValues.Attributes;

namespace HsManCommonLibrary;

class AConfig : ISyncableConfig
{
    public IConfigurationRegistryCenter RegistryCenter => Global.ConfigurationRegistryCenter;
    [AutoAssign("main.test.var0")]
    public int TestInt1 { get; set; } = 0;
    
    [AutoAssign("main.test.var1")]
    public int TestInt2 { get; set; } = 1;
    public void Save()
    {
        ConfigAssigner.AssignFrom(this);
    }
}