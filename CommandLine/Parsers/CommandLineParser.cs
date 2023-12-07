using System.Text;

namespace HsManCommonLibrary.CommandLine.Parsers;

public class CommandLineParser
{
    public List<string> ArgumentPrefixes { get; } = new List<string>()
    {
        "-", "--"
    };

    string ReadString(string commandLine, ref int i)
    {
        if (commandLine[i] != '"')
        {
            throw new FormatException("Expected \"");
        }
        i++;
        StringBuilder builder = new StringBuilder();
        while(commandLine[i] != '"')
        {
            builder.Append(commandLine[i]);
            i++;
        }
        
        return builder.ToString();
    }
    
    public ParsedArgument[] Parse(string commandLine)
    {
        List<string> tokens = new List<string>();
        StringBuilder lastToken = new StringBuilder();
        
        for (int i = 0; i < commandLine.Length; i++)
        {
            switch (commandLine[i])
            {
                case '"':
                    tokens.Add(ReadString(commandLine, ref i));
                    break;
                case ' ':
                    if (lastToken.Length == 0)
                    {
                        continue;
                    }
                    
                    tokens.Add(lastToken.ToString());
                    lastToken.Clear();
                    break;
                default:
                    lastToken.Append(commandLine[i]);
                    break;
            }
        }

        if (lastToken.Length > 0)
        {
            tokens.Add(lastToken.ToString());
        }
        
        
        return Parse(tokens.ToArray());
    }
    
    public ParsedArgument[] Parse(string[] commandLine)
    {
        List<ParsedArgument> parsedCommandLineElements = new List<ParsedArgument>();
        string lastArgumentKey = "";
        ArgumentPrefixes.Sort((x, y) => y.Length.CompareTo(x.Length));
        for (int i = 0; i < commandLine.Length; i++)
        {
            foreach (var argumentPrefix in ArgumentPrefixes)
            {
                if (commandLine[i].StartsWith(argumentPrefix))
                {
                    if (!string.IsNullOrEmpty(lastArgumentKey))
                    {
                        ParsedArgument element = new ParsedArgument(lastArgumentKey, null);
                        parsedCommandLineElements.Add(element);
                    }
                
                    lastArgumentKey = commandLine[i];
                    break;
                }

                if (string.IsNullOrEmpty(lastArgumentKey))
                {
                    continue;
                }
            
                if (!commandLine[i].StartsWith(argumentPrefix))
                {
                    ParsedArgument element = new ParsedArgument(lastArgumentKey, commandLine[i]);
                    lastArgumentKey = "";
                    parsedCommandLineElements.Add(element);
                }
                else
                {
                    ParsedArgument element = new ParsedArgument(lastArgumentKey, null);
                    lastArgumentKey = "";
                    i--;
                    parsedCommandLineElements.Add(element);
                }
            }
        }

        if (string.IsNullOrEmpty(lastArgumentKey))
        {
            return parsedCommandLineElements.ToArray();
        }
        
        ParsedArgument element1 = new ParsedArgument(lastArgumentKey, null);
        parsedCommandLineElements.Add(element1);
        return parsedCommandLineElements.ToArray();
    }
}