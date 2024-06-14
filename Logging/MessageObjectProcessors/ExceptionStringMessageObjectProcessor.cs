using System.Text;

namespace HsManCommonLibrary.Logging.MessageObjectProcessors;

public class ExceptionStringMessageObjectProcessor : IMessageObjectProcessor<Exception, string>
{
    public string ProcessedMessageObject(Exception input, bool processInherited)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine($"ExceptionType: {input.GetType()}");
        builder.AppendLine($"Message: {input.Message}");
        return builder.ToString();
    }

    public object? ProcessedMessageObject(object? input, bool processInherited)
    {
        if (input == null)
        {
            return null;
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