namespace CommonLibrary.Utils;

public static class ConsoleUtils
{
    static Dictionary<ConsoleColor, string> _colorSeqDictionary = new Dictionary<ConsoleColor, string>()
    {
        { ConsoleColor.Black, "\x1b[30m" },
        { ConsoleColor.DarkBlue, "\x1b[34m" },
        { ConsoleColor.DarkGreen, "\x1b[32m" },
        { ConsoleColor.DarkCyan, "\x1b[36m" },
        { ConsoleColor.DarkRed, "\x1b[31m" },
        { ConsoleColor.DarkMagenta, "\x1b[35m" },
        { ConsoleColor.DarkYellow, "\x1b[33m" },
        { ConsoleColor.Gray, "\x1b[37m" },
        { ConsoleColor.DarkGray, "\x1b[90m" },
        { ConsoleColor.Blue, "\x1b[94m" },
        { ConsoleColor.Green, "\x1b[92m" },
        { ConsoleColor.Cyan, "\x1b[96m" },
        { ConsoleColor.Red, "\x1b[91m" },
        { ConsoleColor.Magenta, "\x1b[95m" },
        { ConsoleColor.Yellow, "\x1b[93m" },
        { ConsoleColor.White, "\x1b[97m" }
    };

    public static string GetColorSequence(ConsoleColor consoleColor)
    {
        return _colorSeqDictionary[consoleColor];
    }

    public static string ColorEndSequence => "\x1b[0m";
}