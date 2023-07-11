using HsManCommonLibrary.Logger.Renderers;
using HsManCommonLibrary.Utils;

namespace HsManCommonLibrary.Logger.Serializers;

public class ConsoleLevelObjectSerializer : IObjectSerializer<Level>
{
    public void Serialize(ColoredStringContainer container, Level obj)
    {
        container.Append("[", ColorUtils.FromConsoleColor(ConsoleColor.Gray));
        switch (obj.Name)
        {
            case "Info":
            case "Trace":
            case "Verbose":
                container.Append(obj.Name, ColorUtils.FromConsoleColor(ConsoleColor.Blue));
                break;
            case "Warning":
                container.Append(obj.Name, ColorUtils.FromConsoleColor(ConsoleColor.Yellow));
                break;
            case "Debug":
                container.Append(obj.Name, ColorUtils.FromConsoleColor(ConsoleColor.Green));
                break;
            case "Error":
            case "Exception":
                container.Append(obj.Name, ColorUtils.FromConsoleColor(ConsoleColor.Red));
                break;
            case "Fatal":
                container.Append(obj.Name, ColorUtils.FromConsoleColor(ConsoleColor.DarkRed));
                break;
            default: 
                container.Append(obj.Name, ColorUtils.FromConsoleColor(ConsoleColor.Blue));
                break;
        }
        container.Append("] ", ColorUtils.FromConsoleColor(ConsoleColor.Gray));
    }

    public void Serialize(ColoredStringContainer container, object obj)
    {
        Serialize(container, obj as Level ?? throw new InvalidCastException());
    }
}