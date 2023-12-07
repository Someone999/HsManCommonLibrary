namespace HsManCommonLibrary.CommandLine.Matchers;

public class ArgumentDescriptor
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string? DefaultValue { get; set; }
    public List<string> Options { get; set; } = new List<string>();
    public string? HelpText { get; set; } = "";
    
}