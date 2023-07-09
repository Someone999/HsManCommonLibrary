using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace CommonLibrary.Utils;

public static class JsonUtils
{
    
    
    public static string SerializeObject(object obj)
    {
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };
        
        return JsonConvert.SerializeObject(obj, settings);
    }
    
    public static string SerializeObjectWithLowerCase(object obj)
    {
        var contractResolver = new CamelCasePropertyNamesContractResolver();
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = contractResolver
        };
        
        return JsonConvert.SerializeObject(obj, settings);
    }
}