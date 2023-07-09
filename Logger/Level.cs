namespace CommonLibrary.Logger;

public class Level
{
    public Level(string name, int priority)
    {
        Name = name;
        Priority = priority;
    }

    public int Priority { get; set; }
    public string Name { get; set; }

    public static Level Verbose { get; } = new("Verbose", 0);
    public static Level Trace { get; } = new("Verbose", 1);
    public static Level Info { get; } = new("Info", 2);
    public static Level Debug { get; } = new("Debug", 3);
    public static Level Notice { get; } = new("Notice", 4);
    public static Level Warning { get; } = new("Warning", 5);
    public static Level Error { get; } = new("Error", 6);
    public static Level Exception { get; } = new("Exception", 7);
    public static Level Fatal { get; } = new("Fatal", 8);
    
}