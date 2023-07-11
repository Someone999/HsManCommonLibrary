using Newtonsoft.Json.Linq;

namespace HsManCommonLibrary.ConfigElements.ConfigAdapters;

public class JsonConfigElementAdapter : IConfigElementAdapter
{
    public IConfigElement ToConfigElement(object? obj)
    {
        switch (obj)
        { 
            case JObject jObject:
                object jsonDict = jObject.ToObject<Dictionary<string, object>>() ?? (object)NullConfigValue.Value;
                return new CommonConfigElement(jsonDict);
            case JProperty property:
                return new CommonConfigElement(property.Value);
            case JValue value:
                object val = value.Value ?? NullConfigValue.Value;

                return new CommonConfigElement(val);
            case JArray array:
                object val1 = array.ToObject<List<object>>() ?? (object)NullConfigValue.Value;
                return new CommonConfigElement(val1);
            default: throw new InvalidCastException();
        }
    }
        

    public bool CanConvert(Type t)
    {
        return t == typeof(JObject) || t == typeof(JArray) || t == typeof(JProperty) || t == typeof(JValue);
    }
}