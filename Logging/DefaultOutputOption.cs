using HsManCommonLibrary.Logging.MessageObjectProcessors;

namespace HsManCommonLibrary.Logging;

public class DefaultOutputOption : IOutputOptions
{
    public bool LogTime { get; set; } = true;
    public MessageObjectProcessorManager MessageObjectProcessors { get; } =
        new MessageObjectProcessorManager();
}