using HsManCommonLibrary.Utils;

namespace HsManCommonLibrary.Logging;

public interface IConsoleOutputOptions : IOutputOptions
{
    Dictionary<LogLevel, ConsoleTextColorEscape> LevelForegroundColors { get; }
    Dictionary<LogLevel, ConsoleBackgroundColorEscape> LevelBackgroundColors { get; }
    ConsoleTextColorEscape DefaultTextColor { get; }
    ConsoleBackgroundColorEscape DefaultBackgroundColor { get; }
}