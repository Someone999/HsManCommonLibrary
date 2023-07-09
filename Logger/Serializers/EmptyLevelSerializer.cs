using CommonLibrary.Logger.Renderers;

namespace CommonLibrary.Logger.Serializers;

public class EmptyLevelSerializer : IObjectSerializer<Level>
{
    public void Serialize(ColoredStringContainer container, Level obj)
    {
    }

    public void Serialize(ColoredStringContainer container, object obj)
    {
    }
}