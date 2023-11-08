using Newtonsoft.Json.Linq;

namespace HsManCommonLibrary.NestedValues.NestedValueConverters;

public class JObjectNestedValueStoreConverter : INestedValueStoreConverter<JObject>
{
    JObject ExpendObject(INestedValueStore nestedValueStore)
    {
        var dict = nestedValueStore.GetValueAs<Dictionary<string, object>>();
        if (dict == null)
        {
            throw new InvalidOperationException();
        }
        
        var jObject = new JObject();
        foreach (var pair in dict)
        {
            var realVal = pair.Value;
            switch (realVal)
            {
                case INestedValueStore valueStore:
                    var storedValue = valueStore.GetValue();
                    switch (storedValue)
                    {
                        case Dictionary<string, object>:
                            jObject.Add(pair.Key, ExpendObject(valueStore));
                            break;
                        case List<object>:
                            jObject.Add(pair.Key, ExpendArray(valueStore));
                            break;
                        default:
                            storedValue = Equals(storedValue, NullObject.Value) ? null : storedValue;
                            jObject.Add(pair.Key, storedValue == null ? JValue.CreateNull() : JToken.FromObject(storedValue));
                            break;
                    }

                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        return jObject;
    }
    
    
    JArray ExpendArray(INestedValueStore nestedValueStore)
    {
        var realVal = nestedValueStore.GetValue();
        if (realVal is not List<object> list)
        {
            throw new InvalidOperationException();
        }
        
        var jArray = new JArray();
        foreach (var obj in list)
        {
            jArray.Add(obj);
        }

        return jArray;
    }
    
    
    public JObject Convert(INestedValueStore nestedValueStore)
    {
        return ExpendObject(nestedValueStore);
    }

    object? INestedValueStoreConverter.Convert(INestedValueStore nestValueStore)
    {
        return Convert(nestValueStore);
    }
}