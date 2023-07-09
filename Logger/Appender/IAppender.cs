using CommonLibrary.Logger.Renderers;

namespace CommonLibrary.Logger.Appender;

public interface IAppender
{
    IStringRenderer Renderer { get; set; }
    bool SupportColoredText { get; }
    void Append(LoggingEvent loggingEvent);
}