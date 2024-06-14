using HsManCommonLibrary.Logging.Appenders;
using HsManCommonLibrary.Logging.Loggers;

namespace HsManCommonLibrary.Logging.LoggerFactories;

public interface ILoggerFactory
{
    public void RegisterAppender(IAppender appender);
    public IAppender GetAppender(Type type);
    public T GetAppender<T>() where T: IAppender;
    public IAppender DefaultAppender { get; }
    public ILogger GetLogger(string name);
    public ILogger GetLogger<TAppender>(string name) where TAppender: IAppender;
}