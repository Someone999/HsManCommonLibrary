using HsManCommonLibrary.Logger.Renderers;

namespace HsManCommonLibrary.Utils;

public static class ColorUtils
{
    private static readonly Dictionary<ConsoleColor, string> _consoleColorMap = new Dictionary<ConsoleColor, string>
    {
        { ConsoleColor.Black, "#000000" },
        { ConsoleColor.DarkBlue, "#000080" },
        { ConsoleColor.DarkGreen, "#008000" },
        { ConsoleColor.DarkCyan, "#008080" },
        { ConsoleColor.DarkRed, "#800000" },
        { ConsoleColor.DarkMagenta, "#800080" },
        { ConsoleColor.DarkYellow, "#808000" },
        { ConsoleColor.Gray, "#808080" },
        { ConsoleColor.DarkGray, "#A9A9A9" },
        { ConsoleColor.Blue, "#0000FF" },
        { ConsoleColor.Green, "#008000" },
        { ConsoleColor.Cyan, "#00FFFF" },
        { ConsoleColor.Red, "#FF0000" },
        { ConsoleColor.Magenta, "#FF00FF" },
        { ConsoleColor.Yellow, "#FFFF00" },
        { ConsoleColor.White, "#FFFFFF" }
    };

    public static TextColor FromConsoleColor(ConsoleColor consoleColor)
    {
        return TextColor.Parse(_consoleColorMap[consoleColor], TextColorFormat.SharpHex);
    }

    public static ConsoleColor? ToConsoleColor(TextColor color)
    {
        foreach (var c in _consoleColorMap)
        {
            if (c.Value != color.ToSharpHexString())
            {
                continue;
            }

            return c.Key;
        }

        return null;
    }

}