namespace HsManCommonLibrary.Logging.MessageObjectProcessors;

public class DictionaryProcessedMessageObject : IProcessedMessageObject<Dictionary<string, object?>>
{
    public DictionaryProcessedMessageObject(Dictionary<string, object?> content)
    {
        Content = content;
    }

    public Dictionary<string, object?> Content { get; }
    object IProcessedMessageObject.Content => Content;
}