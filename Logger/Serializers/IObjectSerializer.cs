using HsManCommonLibrary.Logger.Renderers;

namespace HsManCommonLibrary.Logger.Serializers;

public interface IObjectSerializer
{
    void Serialize(ColoredStringContainer container, object obj);
}

public interface IObjectSerializer<in T> : IObjectSerializer
{
    void Serialize(ColoredStringContainer container, T obj);
}