using System.Text;
using HsManCommonLibrary.Utils;

namespace HsManCommonLibrary.Logging.Formatters;

public class ConsoleLoggerFormater : ILoggerFormatter
{
    
    public string Format(LoggingEvent loggingEvent, IOutputOptions outputOptions)
    {
        if (outputOptions is not IConsoleOutputOptions consoleOutputOptions)
        {
            consoleOutputOptions = EmptyConsoleOutputOptions.Instance;
        }
        
        StringBuilder builder = new StringBuilder();
        var level = loggingEvent.LogLevel;
        if (outputOptions.LogTime)
        {
            builder.Append($"<{loggingEvent.LogTime}> ");
        }

        var textColorEscape = consoleOutputOptions.LevelForegroundColors.TryGetValue(level, out var color) 
            ? color 
            : consoleOutputOptions.DefaultTextColor;
        
        var backgroundColorEscape = consoleOutputOptions.LevelBackgroundColors.TryGetValue(level, out var backgroundColor) 
            ? backgroundColor 
            : consoleOutputOptions.DefaultBackgroundColor;

        builder.Append(textColorEscape.EscapeSequence);
        builder.Append(backgroundColorEscape.EscapeSequence);
        builder.Append(level.Name);
        builder.Append(ConsoleColorEscape.Restore.EscapeSequence);

        string? msg = null;
        var inputType = loggingEvent.Message?.GetType();
        var outputType = typeof(string);
        var processor = outputOptions.MessageObjectProcessors.GetProcessor(inputType, outputType, true);
        msg = processor == null 
            ? loggingEvent.Message?.ToString() 
            : processor.ProcessedMessageObject(loggingEvent.Message, true)?.ToString();
        
        
        if (string.IsNullOrEmpty(msg))
        {
            return builder.ToString();
        }

        builder.Append(' ');
        builder.Append(msg);
        return builder.ToString();
    }
}