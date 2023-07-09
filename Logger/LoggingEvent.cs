using CommonLibrary.Logger.LoggerFactories;
using CommonLibrary.Logger.Loggers;

namespace CommonLibrary.Logger;

public class LoggingEvent
{
    public ILoggerFactory LoggerFactory { get; set; } = new EmptyLogFactory();
    public ILogger Logger { get; set; } = new EmptyLogger();
    public Level Level { get; set; } = Level.Info;
    public object? Message { get; set; }
    public string? RenderedString { get; set; }
}