namespace HsManCommonLibrary.Logging.Loggers;

public class EmptyLogger : ILogger
{
    public void Debug(object? message)
    {
    }

    public void Info(object? message)
    {
    }

    public void Warn(object? message)
    {
    }

    public void Error(object? message)
    {
    }

    public void Exception(object? message)
    {
    }

    public void Fatal(object? message)
    {
    }

    public void Log(LoggingEvent loggingEvent)
    {
    }
}