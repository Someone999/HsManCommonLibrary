using HsManCommonLibrary.CommandLine.Parsers;

namespace HsManCommonLibrary.CommandLine.Matchers;

public class IgnoreDuplicateOptionHandlingStrategy : IDuplicateOptionHandlingStrategy
{
    public DuplicateOptionHandlingResult Handle(Dictionary<string, List<ParsedArgument>> commandLineOptions, Dictionary<string, DuplicateOptionInfo> duplicateOptionInfoDict)
    {
        return new DuplicateOptionHandlingResult(true);
    }
}