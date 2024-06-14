using System.Reflection;

namespace HsManCommonLibrary.Utils;

public class ConsoleTextColorEscape : ConsoleColorEscape
{
    private ConsoleTextColorEscape(ConsoleColor consoleColor, string escapeSequence) : base(consoleColor, escapeSequence)
    {
    }

    private static Dictionary<ConsoleColor, ConsoleTextColorEscape>? _propertyInfos;

    private static void InitProperties()
    {
        if (_propertyInfos != null)
        {
            return;
        }

        _propertyInfos = new Dictionary<ConsoleColor, ConsoleTextColorEscape>();
        var properties = typeof(ConsoleTextColorEscape).GetProperties(BindingFlags.Static | BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
            var val = (ConsoleTextColorEscape?) propertyInfo.GetValue(null);
            if (val == null)
            {
                continue;
            }
            
            _propertyInfos.Add(val.ConsoleColor, val);
        }
    }
    
    public static ConsoleTextColorEscape FromConsoleColor(ConsoleColor consoleColor)
    {
        if (_propertyInfos == null)
        {
            InitProperties();
        }
        
        if (_propertyInfos == null)
        {
            throw new Exception();
        }


        return _propertyInfos[consoleColor];

    }
    public static ConsoleTextColorEscape Black { get; } = new ConsoleTextColorEscape(ConsoleColor.Black, "\x1b[30m");
    public static ConsoleTextColorEscape Blue { get; } = new ConsoleTextColorEscape(ConsoleColor.Blue, "\x1b[1;34m");
    public static ConsoleTextColorEscape Green { get; } = new ConsoleTextColorEscape(ConsoleColor.Green, "\x1b[1;32m");
    public static ConsoleTextColorEscape Cyan { get; } = new ConsoleTextColorEscape(ConsoleColor.Cyan, "\x1b[1;36m");
    public static ConsoleTextColorEscape Red { get; } = new ConsoleTextColorEscape(ConsoleColor.Red, "\x1b[1;31m");
    public static ConsoleTextColorEscape Magenta { get; } = new ConsoleTextColorEscape(ConsoleColor.Magenta, "\x1b[1;35m");
    public static ConsoleTextColorEscape Yellow { get; } = new ConsoleTextColorEscape(ConsoleColor.White, "\x1b[1;37m");
    public static ConsoleTextColorEscape White { get; } = new ConsoleTextColorEscape(ConsoleColor.Yellow, "\x1b[1;33m");
    public static ConsoleTextColorEscape DarkGray { get; } = new ConsoleTextColorEscape(ConsoleColor.DarkGray, "\x1b[1;30m");
    public static ConsoleTextColorEscape DarkBlue { get; } = new ConsoleTextColorEscape(ConsoleColor.DarkBlue, "\x1b[34m");
    public static ConsoleTextColorEscape DarkGreen { get; } = new ConsoleTextColorEscape(ConsoleColor.DarkGreen, "\x1b[32m");
    public static ConsoleTextColorEscape DarkCyan { get; } = new ConsoleTextColorEscape(ConsoleColor.DarkCyan, "\x1b[36m");
    public static ConsoleTextColorEscape DarkRed { get; } = new ConsoleTextColorEscape(ConsoleColor.DarkRed, "\x1b[31m");
    public static ConsoleTextColorEscape DarkYellow { get; } = new ConsoleTextColorEscape(ConsoleColor.DarkYellow, "\x1b[33m");
    public static ConsoleTextColorEscape Gray { get; } = new ConsoleTextColorEscape(ConsoleColor.Gray, "\x1b[90m");
    
}