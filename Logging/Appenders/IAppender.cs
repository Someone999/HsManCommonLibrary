namespace HsManCommonLibrary.Logging.Appenders;

public interface IAppender
{
    void Append(string message);
}