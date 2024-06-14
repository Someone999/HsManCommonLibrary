namespace HsManCommonLibrary.Logging;

public class LogLevel
{
    public LogLevel(string name, int priority)
    {
        Name = name;
        Priority = priority;
    }

    public int Priority { get; set; }
    public string Name { get; set; }

    public static LogLevel Verbose { get; } = new("Verbose", 0);
    public static LogLevel Trace { get; } = new("Trace", 1);
    public static LogLevel Info { get; } = new("Info", 2);
    public static LogLevel Debug { get; } = new("Debug", 3);
    public static LogLevel Notice { get; } = new("Notice", 4);
    public static LogLevel Warning { get; } = new("Warning", 5);
    public static LogLevel Error { get; } = new("Error", 6);
    public static LogLevel Exception { get; } = new("Exception", 7);
    public static LogLevel Fatal { get; } = new("Fatal", 8);
    
}