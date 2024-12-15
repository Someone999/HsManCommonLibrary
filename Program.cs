using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using HsManCommonLibrary.Logging;
using HsManCommonLibrary.Logging.Appenders;
using HsManCommonLibrary.Logging.Formatters;
using HsManCommonLibrary.Logging.Loggers;
using HsManCommonLibrary.Logging.MessageObjectProcessors;
using HsManCommonLibrary.NestedValues.NestedValueAdapters;
using HsManCommonLibrary.Reflections;
using HsManCommonLibrary.Reflections.Accessors;
using HsManCommonLibrary.Reflections.Finders;
using HsManCommonLibrary.ValueHolders;
using YamlDotNet.Serialization;

namespace HsManCommonLibrary;

class Program
{
    private static void AddSpace(int count, StringBuilder builder)
    {
        for (int i = 0; i < count; i++)
        {
            builder.Append(' ');
        }
    }
    private static void ListFiles(string directory, int indent, StringBuilder list, HashSet<string> exclude)
    {
        if (exclude.Any(directory.Contains))
        {
            return;
        }

        var entries = Directory.GetFileSystemEntries(directory, "*", SearchOption.TopDirectoryOnly);
        foreach (var entry in entries)
        {
            if (exclude.Any(s => entry.Contains(s)))
            {
                continue;
            }
            
            if (File.Exists(entry))
            {
                AddSpace(indent, list);
                var fileName = Path.GetFileName(entry);
                list.AppendLine(fileName);
            }
            else if (Directory.Exists(entry))
            { 
                AddSpace(indent, list);
                var fileName = Path.GetFileName(entry);
                list.AppendLine(fileName);
                ListFiles(entry, indent + 4, list, exclude);
            }
        }
    }
    static void Main(string[] args)
    {
        string dir = @"L:\Code\HsManCommonLibrary";
        HashSet<string> excludeDirs = new HashSet<string>()
        {
            ".vs",
            ".git",
            ".idea",
            "bin",
            "obj",
            "Obsoleted"
        };

        ObservableCollection<string> a;
        
        StringBuilder builder = new StringBuilder();
        ListFiles(dir, 0, builder, excludeDirs);
        Console.WriteLine(builder);
    }
}