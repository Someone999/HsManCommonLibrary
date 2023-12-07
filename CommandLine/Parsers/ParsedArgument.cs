namespace HsManCommonLibrary.CommandLine.Parsers;

public class ParsedArgument
{
    public ParsedArgument(string argumentKey, string? argumentValue)
    {
        ArgumentKey = argumentKey;
        ArgumentValue = argumentValue;
    }
    
    public string ArgumentKey { get; }
    public string? ArgumentValue { get; }
}