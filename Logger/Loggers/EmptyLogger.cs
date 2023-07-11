using HsManCommonLibrary.Logger.Appender;
using HsManCommonLibrary.Logger.LoggerFactories;
using HsManCommonLibrary.Logger.Serializers;

namespace HsManCommonLibrary.Logger.Loggers;

public class EmptyLogger : ILogger
{
    public IObjectSerializer<Level> LevelSerializer { get; } = new EmptyLevelSerializer();
    public ILoggerFactory LoggerFactory { get; } = new EmptyLogFactory();
    public IAppender Appender { get; } = new EmptyAppender();
    public void Log(object message, Level level, bool logTime = true, bool logMethodName = false)
    {
    }
}