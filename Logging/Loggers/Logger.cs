using HsManCommonLibrary.Logging.Appenders;
using HsManCommonLibrary.Logging.Formatters;

namespace HsManCommonLibrary.Logging.Loggers;

public class Logger : ILogger
{
    private readonly ILoggerFormatter _formatter;
    private readonly IAppender _appender;
    private readonly IOutputOptions _outputOptions;

    public Logger(ILoggerFormatter formatter, IAppender appender, IOutputOptions outputOptions)
    {
        _formatter = formatter;
        _appender = appender;
        _outputOptions = outputOptions;
    }


    public void Debug(object? message)
    {
       Log(LogLevel.Debug, message);
    }

    public void Info(object? message)
    {
        Log(LogLevel.Info, message);
    }

    public void Warn(object? message)
    {
        Log(LogLevel.Warning, message);
    }

    public void Error(object? message)
    {
        Log(LogLevel.Error, message);
    }
    
    public void Exception(object? message)
    {
        Log(LogLevel.Exception, message);
    }
    
    public void Fatal(object? message)
    {
        Log(LogLevel.Fatal, message);
    }

    private void Log(LogLevel logLevel, object? obj)
    {
        Log(new LoggingEvent()
        {
            LogLevel = logLevel,
            Message = obj,
            Logger = this,
            LogTime = DateTime.Now
        });
    }

    public void Log(LoggingEvent loggingEvent)
    {
        var formatted = _formatter.Format(loggingEvent, _outputOptions);
        _appender.Append(formatted);
    }
}