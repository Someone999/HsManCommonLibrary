using System.Collections;
using System.Xml.Serialization;
using HsManCommonLibrary.Reflections;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace HsManCommonLibrary.Logging.Formatters;



public class JsonLoggerFormatter : ILoggerFormatter
{
    public string Format(LoggingEvent loggingEvent, IOutputOptions outputOptions)
    {
        var inputType = loggingEvent.Message?.GetType();
        var processor = outputOptions.MessageObjectProcessors.GetProcessor(inputType, true);
        if (processor != null)
        {
            loggingEvent.Message = processor.ProcessedMessageObject(loggingEvent.Message, true);
        }
        
        Dictionary<string, object?> logObject = new Dictionary<string, object?>
        {
            { "level", loggingEvent.LogLevel.Name },
            { "time", loggingEvent.LogTime },
            { "data", loggingEvent.Message }
        };
        
        return JsonConvert.SerializeObject(logObject, Formatting.Indented);
    }
}