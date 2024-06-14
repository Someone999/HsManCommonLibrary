namespace HsManCommonLibrary.Logging.Formatters;

public interface ILoggerFormatter
{
    string Format(LoggingEvent loggingEvent, IOutputOptions outputOptions);
}