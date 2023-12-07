using HsManCommonLibrary.CommandLine.Parsers;

namespace HsManCommonLibrary.CommandLine.Matchers;

public class DuplicateOptionInfo
{
    public DuplicateOptionInfo(ArgumentDescriptor descriptor, ParsedArgument firstOption)
    {
        Descriptor = descriptor;
        FirstOption = firstOption;
    }

    public ArgumentDescriptor Descriptor { get; }
    public ParsedArgument FirstOption { get; }
    public List<ParsedArgument> DuplicateOptions { get; } = new List<ParsedArgument>();

}