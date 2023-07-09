using CommonLibrary.Logger.Appender;
using CommonLibrary.Logger.LoggerFactories;
using CommonLibrary.Logger.Serializers;

namespace CommonLibrary.Logger.Loggers;

public interface ILogger
{ 
   IObjectSerializer<Level> LevelSerializer { get; }
   ILoggerFactory LoggerFactory { get; }
   IAppender Appender { get; }
   void Log(object message, Level level, bool logTime = true, bool logMethodName = false);
}