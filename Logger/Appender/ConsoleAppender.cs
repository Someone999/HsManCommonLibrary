using HsManCommonLibrary.Logger.Renderers;

namespace HsManCommonLibrary.Logger.Appender;

public class ConsoleAppender : IAppender
{
    public IStringRenderer Renderer { get; set; } = new ConsoleStringRender();
    public bool SupportColoredText => true;

    public void Append(LoggingEvent loggingEvent)
    {
        Console.WriteLine(loggingEvent.RenderedString);
    }
    
}