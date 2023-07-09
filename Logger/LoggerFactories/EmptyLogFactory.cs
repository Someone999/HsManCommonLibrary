using CommonLibrary.Logger.Appender;
using CommonLibrary.Logger.Loggers;
using CommonLibrary.Logger.Serializers;

namespace CommonLibrary.Logger.LoggerFactories;

public class EmptyLogFactory : ILoggerFactory
{
    public SerializerMap SerializerMap { get; } = new SerializerMap();
    public IObjectSerializer DefaultObjectSerializer { get; } = new EmptyLevelSerializer();
    public void RegisterObjectSerializer<T>(IObjectSerializer<T> serializer)
    {
    }

    public IObjectSerializer? GetSerializer(Type t)
    {
        return default;
    }

    public IObjectSerializer<T>? GetSerializer<T>() where T : IObjectSerializer
    {
        return default;
    }

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