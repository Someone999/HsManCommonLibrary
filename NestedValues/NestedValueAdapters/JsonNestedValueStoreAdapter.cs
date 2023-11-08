using Newtonsoft.Json.Linq;

namespace HsManCommonLibrary.NestedValues.NestedValueAdapters;

public class JsonNestedValueStoreAdapter : INestedValueStoreAdapter
{

    private INestedValueStore Expend(JToken? jToken)
    {
        if (jToken == null)
        {
            return new CommonNestedValueStore(NullObject.Value);
        }
        
        switch (jToken)
        {
            case JObject jObj:
                return ExpendJObject(jObj);
            case JProperty jProperty:
                return new CommonNestedValueStore(jProperty.Value);
            case JValue jValue:
                object val = jValue.Value ?? NullObject.Value;
                return new CommonNestedValueStore(val);
            case JArray jArray:
                List<INestedValueStore>? nestedValueStores = new List<INestedValueStore>();
                foreach (var item in jArray)
                {
                    nestedValueStores.Add(Expend(item));
                }

                return new CommonNestedValueStore(nestedValueStores);
        }

        throw new NotSupportedException();
    }
    private INestedValueStore ExpendJObject(JObject jObject)
    {
        Dictionary<string, INestedValueStore>? result = new Dictionary<string, INestedValueStore>();
        foreach (var pair in jObject)
        {
            result.Add(pair.Key, Expend(pair.Value));
        }

        return new CommonNestedValueStore(result);
    }
    public INestedValueStore ToNestedValue(object? obj)
    {
        if (obj is not JToken token)
        {
            throw new NotSupportedException();
        }

        return Expend(token);
    }
        

    public bool CanConvert(Type t)
    {
        return t == typeof(JObject) || t == typeof(JArray) || t == typeof(JProperty) || t == typeof(JValue);
    }
}