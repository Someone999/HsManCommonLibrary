using System.Globalization;
using System.Text;
using System.Xml;

namespace HsManCommonLibrary.Logging.Formatters;

public class XmlLoggerFormatter : ILoggerFormatter
{
    private void GenerateXml(XmlWriter writer, object? obj)
    {
        switch (obj)
        {
            case null:
                writer.WriteValue(string.Empty);
                break;
            case string s:
                writer.WriteValue(s);
                break;
            case IFormattable formattable:
                writer.WriteValue(formattable.ToString(null, CultureInfo.InvariantCulture));
                break;
            case IConvertible convertible:
                writer.WriteValue(convertible.ToString(CultureInfo.InvariantCulture));
                break;
            case IDictionary<string, object?> dict:
                foreach (var o in dict)
                {
                    var tmpName = o.Key;
                    if (char.IsLower(tmpName[0]))
                    {
                        tmpName = char.ToUpper(tmpName[0]) + tmpName.Substring(1);
                    }

                    writer.WriteStartElement(tmpName);
                    GenerateXml(writer, o.Value);
                    writer.WriteEndElement();
                }
                break;
            default:
            {
                Type t = obj.GetType();
                var properties = t.GetProperties();
                foreach (var property in properties)
                {
                    bool shouldWriteValue = true;
                    object? val = null;
                    try
                    {
                        val = property.GetValue(obj);
                    }
                    catch (Exception)
                    {
                        shouldWriteValue = false;
                    }

                    if (!shouldWriteValue || val == null)
                    {
                        continue;
                    }

                    var tmpName = property.Name;
                    if (char.IsLower(tmpName[0]))
                    {
                        tmpName = char.ToUpper(tmpName[0]) + tmpName.Substring(1);
                    }
                    
                    writer.WriteStartElement(tmpName);
                    GenerateXml(writer, val);
                    writer.WriteEndElement();
                }
                break;
            }
        }
    }

    private void GenerateLogXml(XmlWriter writer, Dictionary<string, object?> dictionary)
    {
        var lowerLevel = ((string?)dictionary["level"])?.ToLower() ?? "info";
        var logTime = ((DateTime?)dictionary["time"] ?? DateTime.Now).ToString(CultureInfo.InvariantCulture);
        writer.WriteStartElement("Log");
        writer.WriteAttributeString("level", lowerLevel);
        writer.WriteAttributeString("time", logTime);
        writer.WriteStartElement("Data");
        GenerateXml(writer, dictionary["data"]);
        writer.WriteEndElement();
        writer.WriteEndElement();
    }
    public string Format(LoggingEvent loggingEvent, IOutputOptions outputOptions)
    {
        MemoryStream memoryStream = new MemoryStream();
        XmlWriter writer = XmlWriter.Create(memoryStream, new XmlWriterSettings()
        {
            Indent = true,
            IndentChars = "    ",
            NewLineOnAttributes = false,
            OmitXmlDeclaration = true,
            Encoding = Encoding.UTF8
        });
        
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

        GenerateLogXml(writer, logObject);
        writer.Flush();
        memoryStream.Write(new[] { (byte)'\n' }, 0, 1);
        var bytes = memoryStream.ToArray();
        int startIndex = 0, length = bytes.Length;
        if (!HasUtf8Bom(bytes))
        {
            return Encoding.UTF8.GetString(bytes, startIndex, length);
        }
        
        startIndex = 3;
        length -= 3;

        return Encoding.UTF8.GetString(bytes, startIndex, length);
    }

    bool HasUtf8Bom(byte[] bytes) => bytes.Length > 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF;
}