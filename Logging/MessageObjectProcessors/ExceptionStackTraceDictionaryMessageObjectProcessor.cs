namespace HsManCommonLibrary.Logging.MessageObjectProcessors;

public class ExceptionStackTraceDictionaryMessageObjectProcessor : IMessageObjectProcessor<Exception, Dictionary<string, object?>>
{
    public Dictionary<string, object?> ProcessedMessageObject(Exception input, bool processInherit)
    {
        return new Dictionary<string, object?>
        {
            {"exceptionType", input.GetType().ToString()},
            {"message", input.Message},
            {"stackTrace", input.StackTrace}
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