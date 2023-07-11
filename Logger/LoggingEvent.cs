using HsManCommonLibrary.Logger.LoggerFactories;
using HsManCommonLibrary.Logger.Loggers;

namespace HsManCommonLibrary.Logger;

public class LoggingEvent
{
    public ILoggerFactory LoggerFactory { get; set; } = new EmptyLogFactory();
    public ILogger Logger { get; set; } = new EmptyLogger();
    public Level Level { get; set; } = Level.Info;
    public object? Message { get; set; }
    public string? RenderedString { get; set; }
}