using HsManCommonLibrary.Logging.LoggerFactories;
using HsManCommonLibrary.Logging.Loggers;

namespace HsManCommonLibrary.Logging;

public class LoggingEvent
{
    public ILoggerFactory LoggerFactory { get; set; } = new EmptyLogFactory();
    public ILogger Logger { get; set; } = new EmptyLogger();
    public LogLevel LogLevel { get; set; } = LogLevel.Info;
    public DateTime LogTime { get; set; } = DateTime.Now;
    public object? Message { get; set; }
}

