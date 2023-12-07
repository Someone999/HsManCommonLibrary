using HsManCommonLibrary.CommandLine.Parsers;

namespace HsManCommonLibrary.CommandLine.Matchers;

public class DuplicateOptionHandlingResult
{
    public DuplicateOptionHandlingResult(bool continueExecute, string? errorMassage = null)
    {
        ContinueExecute = continueExecute;
        ErrorMassage = errorMassage;
    }
    public bool ContinueExecute { get; }
    public string? ErrorMassage { get; }
}

public interface IDuplicateOptionHandlingStrategy
{
    public DuplicateOptionHandlingResult Handle(Dictionary<string, List<ParsedArgument>> commandLineOptions,
        Dictionary<string, DuplicateOptionInfo> duplicateOptionInfoDict);
}

public class RejectDuplicateOptionHandlingStrategy : IDuplicateOptionHandlingStrategy
{
    public DuplicateOptionHandlingResult Handle(Dictionary<string, List<ParsedArgument>> commandLineOptions, Dictionary<string, DuplicateOptionInfo> duplicateOptionInfoDict)
    {
        return new DuplicateOptionHandlingResult(false,  "Duplicate argument is not allowed");
    }
}

public class UseLastOptionDuplicateOptionHandlingStrategy : IDuplicateOptionHandlingStrategy
{
    public DuplicateOptionHandlingResult Handle(Dictionary<string, List<ParsedArgument>> commandLineOptions, Dictionary<string, DuplicateOptionInfo> duplicateOptionInfoDict)
    {
        var commandLineOptionList = commandLineOptions.ToList();
        foreach (var duplicateOptionInfo in duplicateOptionInfoDict)
        {
            var duplicated = duplicateOptionInfo.Value.DuplicateOptions;
            var removeList = duplicated.Take(duplicated.Count - 1);
            foreach (var argument in removeList)
            {
                var toRemove = commandLineOptions[duplicateOptionInfo.Key]
                    .FirstOrDefault(p => p.ArgumentKey == argument.ArgumentKey);
                if (toRemove == null)
                {
                    continue;
                }

                commandLineOptions[duplicateOptionInfo.Key].Remove(toRemove);
            }
        }

        return new DuplicateOptionHandlingResult(true);
    }
}

public class UseFirstOptionDuplicateOptionHandlingStrategy : IDuplicateOptionHandlingStrategy
{
    public DuplicateOptionHandlingResult Handle(Dictionary<string, List<ParsedArgument>> commandLineOptions, Dictionary<string, DuplicateOptionInfo> duplicateOptionInfoDict)
    {
        var commandLineOptionList = commandLineOptions.ToList();
        foreach (var duplicateOptionInfo in duplicateOptionInfoDict)
        {
            var duplicated = duplicateOptionInfo.Value.DuplicateOptions;
            var removeList = duplicated.Skip(1);
            foreach (var argument in removeList)
            {
                var toRemove = commandLineOptions[duplicateOptionInfo.Key]
                    .FirstOrDefault(p => p.ArgumentKey == argument.ArgumentKey);
                if (toRemove == null)
                {
                    continue;
                }

                commandLineOptions[duplicateOptionInfo.Key].Remove(toRemove);
            }
        }

        return new DuplicateOptionHandlingResult(true);
    }
}

public class IgnoreDuplicateOptionHandlingStrategy : IDuplicateOptionHandlingStrategy
{
    public DuplicateOptionHandlingResult Handle(Dictionary<string, List<ParsedArgument>> commandLineOptions, Dictionary<string, DuplicateOptionInfo> duplicateOptionInfoDict)
    {
        return new DuplicateOptionHandlingResult(true);
    }
}