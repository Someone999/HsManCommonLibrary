using System.Text;
using HsManCommonLibrary.Utils;

namespace HsManCommonLibrary.Logger.Renderers;

public class ConsoleStringRender : IStringRenderer
{
    static string ConvertToConsoleControlSequence(ColoredString coloredString)
    {
        var consoleColor = ColorUtils.ToConsoleColor(coloredString.Color);
        StringBuilder builder = new StringBuilder();
        builder.Append(ConsoleUtils.GetColorSequence(consoleColor ?? ConsoleColor.Gray));
        builder.Append(coloredString.Text);
        builder.Append(ConsoleUtils.ColorEndSequence);
        return builder.ToString();
    }
    
    public string Render(ColoredStringContainer container, bool supportColor)
    {
        StringWriter writer = new StringWriter();
        foreach (var str in container)
        {
            writer.Write(supportColor ? ConvertToConsoleControlSequence(str) : str.Text);
        }

        return writer.ToString();
    }
}