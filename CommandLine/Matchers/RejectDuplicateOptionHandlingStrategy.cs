using HsManCommonLibrary.CommandLine.Parsers;

namespace HsManCommonLibrary.CommandLine.Matchers;

public class RejectDuplicateOptionHandlingStrategy : IDuplicateOptionHandlingStrategy
{
    public DuplicateOptionHandlingResult Handle(Dictionary<string, List<ParsedArgument>> commandLineOptions, Dictionary<string, DuplicateOptionInfo> duplicateOptionInfoDict)
    {
        return new DuplicateOptionHandlingResult(false,  "Duplicate argument is not allowed");
    }
}