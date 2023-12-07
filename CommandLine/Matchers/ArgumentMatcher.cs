using System.Text;
using HsManCommonLibrary.CommandLine.Parsers;

namespace HsManCommonLibrary.CommandLine.Matchers;

public class ArgumentMatcher
{
    public List<ArgumentDescriptor> Descriptors { get; set; } = new List<ArgumentDescriptor>();
    public string Usage { get; set; } = "";

    public virtual string GetHelp()
    {
        StringBuilder builder = new StringBuilder(Usage);
        builder.AppendLine();
        builder.AppendLine();
        builder.AppendLine("Help: ");
        foreach (var descriptor in Descriptors)
        {
            string descBase = string.Join(", ", descriptor.Options);
            bool hasDefaultVal = descriptor.DefaultValue != null;
            builder.Append($"{descBase}:\t\t{descriptor.Description} Optional: {hasDefaultVal}");
            if (hasDefaultVal)
            {
                builder.AppendLine($" (Default value: {descriptor.DefaultValue})");
            }
            else
            {
                builder.AppendLine();
            }

            if (string.IsNullOrEmpty(descriptor.HelpText))
            {
                continue;
            }
            
            builder.AppendLine("    Option help:");
            builder.AppendLine(descriptor.HelpText);
        }

        return builder.ToString();
    }


    private Dictionary<string, ParsedArgument[]> MatchCommandLineElements(ParsedArgument[] parsedElements,
        IDuplicateOptionHandlingStrategy? duplicateOptionHandlingStrategy)
    {
        Dictionary<string, List<ParsedArgument>> matchedElements = new Dictionary<string, List<ParsedArgument>>();
        Dictionary<string, DuplicateOptionInfo> duplicateOptionInfos = new Dictionary<string, DuplicateOptionInfo>();
        foreach (var elem in parsedElements)
        {
            foreach (var descriptor in Descriptors)
            {
                if (!descriptor.Options.Contains(elem.ArgumentKey))
                {
                    continue;
                }
                
                if (duplicateOptionInfos.ContainsKey(descriptor.Name))
                {
                    duplicateOptionInfos[descriptor.Name].DuplicateOptions.Add(elem);
                }
                else
                {
                    var duplicateInfo = new DuplicateOptionInfo(descriptor, elem);
                    duplicateInfo.DuplicateOptions.Add(elem);
                    duplicateOptionInfos.Add(descriptor.Name, duplicateInfo);
                }
                
                if (matchedElements.ContainsKey(descriptor.Name))
                {
                    matchedElements[descriptor.Name].Add(elem);
                }
                else
                {
                    matchedElements.Add(descriptor.Name, new List<ParsedArgument>(){elem});
                }
            }
        }

        foreach (var argumentDescriptor in Descriptors)
        {
            if (matchedElements.ContainsKey(argumentDescriptor.Name) ||
                argumentDescriptor.DefaultValue == null)
            {
                continue;
            }
            var elem = new ParsedArgument(argumentDescriptor.Options[0], argumentDescriptor.DefaultValue);
            matchedElements.Add(argumentDescriptor.Name, new List<ParsedArgument>(){elem});
        }

        var notDuplicateOptions = duplicateOptionInfos
            .Where(p => p.Value.DuplicateOptions.Count == 1)
            .Select(p => p.Key).ToList();

        foreach (var notDuplicateOption in notDuplicateOptions)
        {
            duplicateOptionInfos.Remove(notDuplicateOption);
        }
        
        if (duplicateOptionInfos.Count == 0 || duplicateOptionHandlingStrategy == null)
        {
            return matchedElements.ToDictionary(k => k.Key, v => v.Value.ToArray());
        }
        
        var result = duplicateOptionHandlingStrategy.Handle(matchedElements, duplicateOptionInfos);
        if (result.ContinueExecute)
        {
            return matchedElements.ToDictionary(k => k.Key, v => v.Value.ToArray());
        }

        if (string.IsNullOrEmpty(result.ErrorMassage))
        {
            Console.WriteLine(result.ErrorMassage);
        }
        
        Console.WriteLine(result.ErrorMassage);
        Environment.Exit(0);

        return new Dictionary<string, ParsedArgument[]>();
    }
    
    public virtual Dictionary<string, ParsedArgument[]> Match(string commandLine,
        IDuplicateOptionHandlingStrategy? strategy)
    {
        CommandLineParser parser = new CommandLineParser();
        return MatchCommandLineElements(parser.Parse(commandLine), strategy);
    }
    
    public virtual Dictionary<string, ParsedArgument[]> Match(string[] commandLine,
        IDuplicateOptionHandlingStrategy? strategy)
    {
        CommandLineParser parser = new CommandLineParser();
        return MatchCommandLineElements(parser.Parse(commandLine), strategy);
    }
    
}