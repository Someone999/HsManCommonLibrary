using HsManCommonLibrary.Configuration;

namespace HsManCommonLibrary;

static class Global
{
    public static IConfigurationRegistryCenter ConfigurationRegistryCenter { get; } = new ConfigurationRegistryCenter();
}