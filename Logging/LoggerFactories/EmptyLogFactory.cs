using HsManCommonLibrary.Logging.Appenders;
using HsManCommonLibrary.Logging.Loggers;

namespace HsManCommonLibrary.Logging.LoggerFactories;

public class EmptyLogFactory : ILoggerFactory
{
    public void RegisterAppender(IAppender appender)
    {
    }

    public IAppender GetAppender(Type type)
    {
        return new EmptyAppender();
    }

    public T GetAppender<T>() where T : IAppender
    {
        throw new NotSupportedException();
    }

    public IAppender DefaultAppender { get; } = new EmptyAppender();
    public ILogger GetLogger(string name)
    {
        return new EmptyLogger();
    }

    public ILogger GetLogger<TAppender>(string name) where TAppender : IAppender
    {
        return new EmptyLogger();
    }
}