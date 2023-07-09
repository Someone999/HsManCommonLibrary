using CommonLibrary.Logger.Appender;
using CommonLibrary.Logger.Loggers;
using CommonLibrary.Logger.Serializers;

namespace CommonLibrary.Logger.LoggerFactories;

public interface ILoggerFactory
{
    SerializerMap SerializerMap { get; }
    IObjectSerializer DefaultObjectSerializer { get; }
    void RegisterObjectSerializer<T>(IObjectSerializer<T> serializer);
    IObjectSerializer? GetSerializer(Type t);
    IObjectSerializer<T>? GetSerializer<T>() where T: IObjectSerializer;
    public void RegisterAppender(IAppender appender);
    public IAppender GetAppender(Type type);
    public T GetAppender<T>() where T: IAppender;
    public IAppender DefaultAppender { get; }
    public ILogger GetLogger(string name);
    public ILogger GetLogger<TAppender>(string name) where TAppender: IAppender;
}