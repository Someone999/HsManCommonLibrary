using HsManCommonLibrary.Logger.Appender;
using HsManCommonLibrary.Logger.LoggerFactories;
using HsManCommonLibrary.Logger.Serializers;

namespace HsManCommonLibrary.Logger.Loggers;

public interface ILogger
{ 
   IObjectSerializer<Level> LevelSerializer { get; }
   ILoggerFactory LoggerFactory { get; }
   IAppender Appender { get; }
   void Log(object message, Level level, bool logTime = true, bool logMethodName = false);
}