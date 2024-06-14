using HsManCommonLibrary.Logging.MessageObjectProcessors;

namespace HsManCommonLibrary.Logging;

public interface IOutputOptions
{
    bool LogTime { get; set; }
    MessageObjectProcessorManager MessageObjectProcessors { get; }
}