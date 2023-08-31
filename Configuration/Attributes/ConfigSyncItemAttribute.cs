namespace HsManCommonLibrary.Configuration.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ConfigSyncItemAttribute : Attribute
{
    public string? ConfigPath { get; set; }
    public Type? ConverterType { get; set; }

    public ConfigSyncItemAttribute(string? configPath = null, Type? converterType = null)
    {
        ConfigPath = configPath;
        ConverterType = converterType;
    }
}