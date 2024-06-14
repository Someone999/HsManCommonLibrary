namespace HsManCommonLibrary.Logging.MessageObjectProcessors;

public interface IMessageObjectProcessor
{
    object? ProcessedMessageObject(object? input, bool processInherited);
}

public interface IProcessedMessageObject<out T> : IProcessedMessageObject
{
    new T Content { get; }
}

public interface IMessageObjectProcessor<in TInput, out TOutput> : IMessageObjectProcessor
{
    public TOutput? ProcessedMessageObject(TInput input, bool processInherited);
}