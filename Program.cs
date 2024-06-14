using HsManCommonLibrary.Logging;
using HsManCommonLibrary.Logging.Appenders;
using HsManCommonLibrary.Logging.Formatters;
using HsManCommonLibrary.Logging.Loggers;
using HsManCommonLibrary.Logging.MessageObjectProcessors;

namespace HsManCommonLibrary;


class Program
{
    
    static void Main(string[] args)
    {
        
        var outputOptions = new DefaultConsoleOutputOption(); 
        outputOptions.MessageObjectProcessors.Register<Exception>(new ExceptionStackTraceDictionaryMessageObjectProcessor());
        Logger logger = new Logger(new XmlLoggerFormatter(), new ConsoleAppender(), outputOptions);
        logger.Exception(new Exception());
    }
}