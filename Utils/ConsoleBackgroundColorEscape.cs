using System.Reflection;

namespace HsManCommonLibrary.Utils;

public class ConsoleBackgroundColorEscape : ConsoleColorEscape
{
    private ConsoleBackgroundColorEscape(ConsoleColor consoleColor, string escapeSequence) : base(consoleColor, escapeSequence)
    {
    }
    
    private static Dictionary<ConsoleColor, ConsoleBackgroundColorEscape>? _propertyInfos;

    private static void InitProperties()
    {
        if (_propertyInfos != null)
        {
            return;
        }

        _propertyInfos = new Dictionary<ConsoleColor, ConsoleBackgroundColorEscape>();
        var properties = typeof(ConsoleBackgroundColorEscape).GetProperties(BindingFlags.Static | BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
            var val = (ConsoleBackgroundColorEscape?) propertyInfo.GetValue(null);
            if (val == null)
            {
                continue;
            }
            
            _propertyInfos.Add(val.ConsoleColor, val);
        }
    }
    
    public static ConsoleBackgroundColorEscape FromConsoleColor(ConsoleColor consoleColor)
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
    
    public static ConsoleBackgroundColorEscape Black { get; } = new ConsoleBackgroundColorEscape(ConsoleColor.Black, "\x1b[40m");
    public static ConsoleBackgroundColorEscape Blue { get; } = new ConsoleBackgroundColorEscape(ConsoleColor.Blue, "\x1b[1;44m");
    public static ConsoleBackgroundColorEscape Green { get; } = new ConsoleBackgroundColorEscape(ConsoleColor.Green, "\x1b[1;42m");
    public static ConsoleBackgroundColorEscape Cyan { get; } = new ConsoleBackgroundColorEscape(ConsoleColor.Cyan, "\x1b[1;46m");
    public static ConsoleBackgroundColorEscape Red { get; } = new ConsoleBackgroundColorEscape(ConsoleColor.Red, "\x1b[1;41m");
    public static ConsoleBackgroundColorEscape Magenta { get; } = new ConsoleBackgroundColorEscape(ConsoleColor.Magenta, "\x1b[1;45m");
    public static ConsoleBackgroundColorEscape Yellow { get; } = new ConsoleBackgroundColorEscape(ConsoleColor.White, "\x1b[1;47m");
    public static ConsoleBackgroundColorEscape White { get; } = new ConsoleBackgroundColorEscape(ConsoleColor.Yellow, "\x1b[1;43m");
    public static ConsoleBackgroundColorEscape DarkGray { get; } = new ConsoleBackgroundColorEscape(ConsoleColor.DarkGray, "\x1b[1;40m");
    public static ConsoleBackgroundColorEscape DarkBlue { get; } = new ConsoleBackgroundColorEscape(ConsoleColor.DarkBlue, "\x1b[44m");
    public static ConsoleBackgroundColorEscape DarkGreen { get; } = new ConsoleBackgroundColorEscape(ConsoleColor.DarkGreen, "\x1b[42m");
    public static ConsoleBackgroundColorEscape DarkCyan { get; } = new ConsoleBackgroundColorEscape(ConsoleColor.DarkCyan, "\x1b[46m");
    public static ConsoleBackgroundColorEscape DarkRed { get; } = new ConsoleBackgroundColorEscape(ConsoleColor.DarkRed, "\x1b[41m");
    public static ConsoleBackgroundColorEscape DarkYellow { get; } = new ConsoleBackgroundColorEscape(ConsoleColor.DarkYellow, "\x1b[43m");
    public static ConsoleBackgroundColorEscape Gray { get; } = new ConsoleBackgroundColorEscape(ConsoleColor.Gray, "\x1b[100m");
    
    
}