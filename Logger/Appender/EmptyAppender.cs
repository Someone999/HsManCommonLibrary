using HsManCommonLibrary.Logger.Renderers;

namespace HsManCommonLibrary.Logger.Appender;

public class EmptyAppender : IAppender
{
    public IStringRenderer Renderer { get; set; } = new EmptyStringRenderer();
    public bool SupportColoredText => false;
    public void Append(LoggingEvent loggingEvent)
    {
    }
}