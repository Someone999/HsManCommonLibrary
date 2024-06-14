using HsManCommonLibrary.Utils;

namespace HsManCommonLibrary.Logging;

public class DefaultConsoleOutputOption : DefaultOutputOption, IConsoleOutputOptions
{
    public Dictionary<LogLevel, ConsoleTextColorEscape> LevelForegroundColors { get; } =
        new Dictionary<LogLevel, ConsoleTextColorEscape>()
        {
            {LogLevel.Verbose, ConsoleTextColorEscape.Gray},
            {LogLevel.Trace, ConsoleTextColorEscape.Green},
            {LogLevel.Info, ConsoleTextColorEscape.Blue},
            {LogLevel.Debug, ConsoleTextColorEscape.DarkGreen},
            {LogLevel.Notice, ConsoleTextColorEscape.Yellow},
            {LogLevel.Warning, ConsoleTextColorEscape.DarkYellow},
            {LogLevel.Error, ConsoleTextColorEscape.Red},
            {LogLevel.Exception, ConsoleTextColorEscape.Red},
            {LogLevel.Fatal, ConsoleTextColorEscape.DarkRed}
        };

    public Dictionary<LogLevel, ConsoleBackgroundColorEscape> LevelBackgroundColors { get; } =
        new Dictionary<LogLevel, ConsoleBackgroundColorEscape>();
    public ConsoleTextColorEscape DefaultTextColor => ConsoleTextColorEscape.FromConsoleColor(Console.ForegroundColor);

    public ConsoleBackgroundColorEscape DefaultBackgroundColor =>
        ConsoleBackgroundColorEscape.FromConsoleColor(Console.BackgroundColor);
}