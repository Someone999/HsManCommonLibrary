using System.Xml;
using HsManCommonLibrary.NestedValues.SaveStrategies;

namespace HsManCommonLibrary.NestedValues.NestedValueAdapters;

public class XmlNestedValueStoreAdapter : INestedValueStoreAdapter
{
    XmlNestedValueStore ToXmlNestedValue(XmlElement element)
    {
        var dict = new Dictionary<string, INestedValueStore>();
        XmlNestedValueStore nestedValueStore = new XmlNestedValueStore(dict);
        var val = new CommonNestedValueStore((object?) element.Value ?? NullNestedValue.Value);
        nestedValueStore.GetValueAs<Dictionary<string, INestedValueStore>>()?.Add(element.Name, val);

        if (element.ChildNodes.Count == 0)
        {
            return nestedValueStore;
        }

        if (element.HasAttributes)
        {
            foreach (XmlAttribute attribute in element.Attributes)
            {
                nestedValueStore.XmlAttributes.Add(attribute.Name, new CommonNestedValueStore(attribute.Value));
            }
        }
        
        foreach (XmlNode childNode in element.ChildNodes)
        {
            if (childNode is not XmlElement xmlElement)
            {
                continue;
            }

            var childDict = nestedValueStore.ChildElements;
            object addValue = ToXmlNestedValue(xmlElement);
            if (childDict.ContainsKey(childNode.Name))
            {
                var storedVal = nestedValueStore.ChildElements[childNode.Name].GetValue();
                if (storedVal is List<INestedValueStore> list)
                {
                    list.Add((INestedValueStore) addValue);
                }
                else
                {
                    var oldVal = addValue;
                    addValue = new List<INestedValueStore>();
                    ((List<INestedValueStore>) addValue).Add((INestedValueStore) oldVal);
                    childDict[childNode.Name] = new CommonNestedValueStore(addValue);
                }
            }
            else
            {
                if (addValue is INestedValueStore nestedValueStore1)
                {
                    childDict.Add(childNode.Name,nestedValueStore1);
                }
                else
                {
                    childDict.Add(childNode.Name, new CommonNestedValueStore(addValue));
                }
            }
        }

        return nestedValueStore;
    }
    
    public INestedValueStore ToNestedValue(object? obj)
    {
        if (!CanConvert(obj?.GetType()) || obj is null)
        {
            throw new NotSupportedException();
        }

        XmlDocument document = new XmlDocument();
        document.LoadXml((string) obj);
        var rootElement = document.DocumentElement;
        return rootElement == null ? new CommonNestedValueStore(NullNestedValue.Value) : ToXmlNestedValue(rootElement);
    }

    public bool CanConvert(Type? t)
    {
        return t == typeof(string);
    }
    
    public static INestedValueStoreAdapter Adapter { get; } = new XmlNestedValueStoreAdapter();
}