using HsManCommonLibrary.Logger.Renderers;

namespace HsManCommonLibrary.Logger.Appender;

public interface IAppender
{
    IStringRenderer Renderer { get; set; }
    bool SupportColoredText { get; }
    void Append(LoggingEvent loggingEvent);
}