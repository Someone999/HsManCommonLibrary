namespace HsManCommonLibrary.Logging.MessageObjectProcessors;

public class ExceptionDictionaryMessageObjectProcessor : IMessageObjectProcessor<Exception, Dictionary<string, object?>>
{
    public Dictionary<string, object?> ProcessedMessageObject(Exception input, bool processInherited)
    {
        return new Dictionary<string, object?>
        {
            {"exceptionType", input.GetType()},
            {"message", input.Message}
        };
    }

    public object ProcessedMessageObject(object? input, bool processInherited)
    {
        if (input == null)
        {
            throw new ArgumentNullException(nameof(input));
        }
        
        if (!processInherited && input.GetType() != typeof(Exception))
        {
            throw new InvalidOperationException($"Type mismatched ({input.GetType()}/{typeof(Exception)}");
        }
        
        if (input is not Exception exception)
        {
            throw new InvalidOperationException($"Type mismatched ({input.GetType()}/{typeof(Exception)}");
        }

        return ProcessedMessageObject(exception, processInherited);
    }
}