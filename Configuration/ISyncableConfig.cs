namespace HsManCommonLibrary.Configuration;

public interface ISyncableConfig
{
    IConfigurationRegistryCenter RegistryCenter { get; }
    void Save();
}