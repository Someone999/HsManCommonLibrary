namespace HsManCommonLibrary.Logging.MessageObjectProcessors;

public class MessageObjectProcessorManager
{
    private Dictionary<Type, IMessageObjectProcessor> _messageObjectProcessors =
        new Dictionary<Type, IMessageObjectProcessor>();

    public void Register<T>(IMessageObjectProcessor messageObjectProcessor)
    {
        _messageObjectProcessors.Add(typeof(T), messageObjectProcessor);
    }
    
    public IMessageObjectProcessor? Replace<T>(IMessageObjectProcessor messageObjectProcessor)
    {
        var type = typeof(T);
        if (!_messageObjectProcessors.TryGetValue(type, out var old))
        {
            return null;
        }

        _messageObjectProcessors[type] = messageObjectProcessor;
        return old;
    }

    private IMessageObjectProcessor? GetProcessorNonInherit(Type type)
    {
        return !_messageObjectProcessors.TryGetValue(type, out var value) ? null : value;
    }

    private IMessageObjectProcessor? GetProcessorProcessInherit(Type type)
    {
        var firstSearch = GetProcessorNonInherit(type);
        if (firstSearch != null)
        {
            return firstSearch;
        }
        
        var value = _messageObjectProcessors
            .FirstOrDefault(t => t.Key.IsAssignableFrom(type)).Value;

        return value;
    }
    
    private IMessageObjectProcessor[] GetProcessorsProcessInherit(Type type)
    {
        var value = _messageObjectProcessors
            .Where(t => t.Key.IsAssignableFrom(type));

        return value.Select(kv => kv.Value).ToArray();
    }
    public IMessageObjectProcessor? GetProcessor(Type? type, bool processInherit)
    {
        if (type == null)
        {
            return null;
        }
        
        return processInherit ? GetProcessorNonInherit(type) : GetProcessorProcessInherit(type);
    }
    
    public IMessageObjectProcessor? GetProcessor<T>(bool processInherit)
    {
        return GetProcessor(typeof(T), processInherit);
    }
    
    public IMessageObjectProcessor? GetProcessor(Type? inputType, Type? outputType, bool processInherit)
    {
        if (inputType == null || outputType == null)
        {
            return null;
        }
        
        var processors = GetProcessorsProcessInherit(inputType);
        var processorType = typeof(IMessageObjectProcessor<,>).MakeGenericType(inputType, outputType);
        var matchedProcessor = processors.FirstOrDefault(p => processorType.IsInstanceOfType(p));
        return matchedProcessor;
    }
    
    public IMessageObjectProcessor? GetProcessor<TInput, TOutput>(bool processInherit)
    {
        var processors = GetProcessorsProcessInherit(typeof(TInput));
        var matchedProcessor = processors.FirstOrDefault(p => p is IMessageObjectProcessor<TInput, TOutput>);
        return matchedProcessor;
    }
}