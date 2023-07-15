using Newtonsoft.Json.Linq;

namespace HsManCommonLibrary.NestedValues.NestedValueAdapters;

public class JsonNestedValueStoreAdapter : INestedValueStoreAdapter
{
    public INestedValueStore ToConfigElement(object? obj)
    {
        switch (obj)
        { 
            case JObject jObject:
                object jsonDict = jObject.ToObject<Dictionary<string, object>>() ?? (object)NullConfigValue.Value;
                return new CommonNestedValueStore(jsonDict);
            case JProperty property:
                return new CommonNestedValueStore(property.Value);
            case JValue value:
                object val = value.Value ?? NullConfigValue.Value;

                return new CommonNestedValueStore(val);
            case JArray array:
                object val1 = array.ToObject<List<object>>() ?? (object)NullConfigValue.Value;
                return new CommonNestedValueStore(val1);
            default: throw new InvalidCastException();
        }
    }
        

    public bool CanConvert(Type t)
    {
        return t == typeof(JObject) || t == typeof(JArray) || t == typeof(JProperty) || t == typeof(JValue);
    }
}