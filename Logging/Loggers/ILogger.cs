namespace HsManCommonLibrary.Logging.Loggers;

public interface ILogger
{
    void Debug(object? message);
    void Info(object? message);
    void Warn(object? message);
    void Error(object? message);
    void Exception(object? message);
    void Fatal(object? message);
    void Log(LoggingEvent loggingEvent);
}