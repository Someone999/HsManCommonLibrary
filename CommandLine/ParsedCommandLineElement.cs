namespace HsManCommonLibrary.CommandLine;

public class ParsedCommandLineElement
{
    public ParsedCommandLineElement(string argumentKey, string? argumentValue)
    {
        ArgumentKey = argumentKey;
        ArgumentValue = argumentValue;
    }
    
    public string ArgumentKey { get; }
    public string? ArgumentValue { get; }
}