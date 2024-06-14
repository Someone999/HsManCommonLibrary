using HsManCommonLibrary.CommandLine.Parsers;

namespace HsManCommonLibrary.CommandLine.Matchers;

public interface IDuplicateOptionHandlingStrategy
{
    public DuplicateOptionHandlingResult Handle(Dictionary<string, List<ParsedArgument>> commandLineOptions,
        Dictionary<string, DuplicateOptionInfo> duplicateOptionInfoDict);
}