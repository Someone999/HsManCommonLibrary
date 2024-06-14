namespace HsManCommonLibrary.Logging.Appenders;

public class FileAppender : IAppender
{
    public string FileName { get; }

    public FileAppender(string fileName)
    {
        FileName = fileName;
    }
    
    public void Append(string message)
    {
        File.AppendAllText(FileName, message);
    }
}