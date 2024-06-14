using HsManCommonLibrary.Logging.MessageObjectProcessors;
using HsManCommonLibrary.Utils;

namespace HsManCommonLibrary.Logging.Formatters;

class EmptyConsoleOutputOptions : IConsoleOutputOptions
{
    public bool LogTime { get; set; }

    public MessageObjectProcessorManager MessageObjectProcessors { get; } =
        new MessageObjectProcessorManager();

    public Dictionary<LogLevel, ConsoleTextColorEscape> LevelForegroundColors { get; } =
        new Dictionary<LogLevel, ConsoleTextColorEscape>();

    public Dictionary<LogLevel, ConsoleBackgroundColorEscape> LevelBackgroundColors { get; }
        = new Dictionary<LogLevel, ConsoleBackgroundColorEscape>();
    public ConsoleTextColorEscape DefaultTextColor => ConsoleTextColorEscape.Gray;
    public ConsoleBackgroundColorEscape DefaultBackgroundColor => ConsoleBackgroundColorEscape.Black;
    internal static IConsoleOutputOptions Instance { get; } = new EmptyConsoleOutputOptions();
}