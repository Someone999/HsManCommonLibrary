namespace HsManCommonLibrary.Logging.Appenders;

public class ConsoleAppender : IAppender
{
    public void Append(string message)
    {
        Console.WriteLine(message);
    }
}