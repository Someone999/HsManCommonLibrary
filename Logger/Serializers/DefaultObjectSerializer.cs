using HsManCommonLibrary.Logger.Renderers;
using HsManCommonLibrary.Utils;

namespace HsManCommonLibrary.Logger.Serializers;

public class DefaultObjectSerializer : IObjectSerializer
{
    public void Serialize(ColoredStringContainer container, object obj)
    {
        container.Append(obj.ToString() ?? "null", ColorUtils.FromConsoleColor(ConsoleColor.Gray));
    }
}