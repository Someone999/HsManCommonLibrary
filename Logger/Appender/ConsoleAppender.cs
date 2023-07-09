using CommonLibrary.Logger.Renderers;

namespace CommonLibrary.Logger.Appender;

public class ConsoleAppender : IAppender
{
    public IStringRenderer Renderer { get; set; } = new ConsoleStringRender();
    public bool SupportColoredText => true;

    public void Append(LoggingEvent loggingEvent)
    {
        Console.WriteLine(loggingEvent.RenderedString);
    }
    
}