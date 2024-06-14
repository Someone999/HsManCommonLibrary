

using System.Globalization;

namespace HsManCommonLibrary.Utils;

public class ConsoleColorEscape
{
    protected ConsoleColorEscape(ConsoleColor consoleColor, string escapeSequence)
    {
        ConsoleColor = consoleColor;
        EscapeSequence = escapeSequence;
    }
    public ConsoleColor ConsoleColor { get; }
    public string EscapeSequence { get; }

    public static ConsoleColorEscape Restore { get; } = new ConsoleColorEscape(0, "\x1b[0m");
}