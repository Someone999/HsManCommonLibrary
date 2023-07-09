using CommonLibrary.Logger.Renderers;
using CommonLibrary.Utils;

namespace CommonLibrary.Logger.Serializers;

public class DefaultObjectSerializer : IObjectSerializer
{
    public void Serialize(ColoredStringContainer container, object obj)
    {
        container.Append(obj.ToString() ?? "null", ColorUtils.FromConsoleColor(ConsoleColor.Gray));
    }
}