using System.Collections;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using HsManCommonLibrary.Collections.Transactional.Transactions;
using HsManCommonLibrary.Collections.Transactional.Transactions.TransactionOperations;
using HsManCommonLibrary.CommandLine.Matchers;
using HsManCommonLibrary.CommandLine.Parsers;
using HsManCommonLibrary.Configuration;
using HsManCommonLibrary.NameStyleConverters;
using HsManCommonLibrary.NestedValues.Attributes;
using HsManCommonLibrary.NestedValues.NestedValueAdapters;
using HsManCommonLibrary.NestedValues.SaveStrategies;
using HsManCommonLibrary.NestedValues.Utils;
using HsManCommonLibrary.PropertySynchronizer;
using HsManCommonLibrary.Reflections;
using HsManCommonLibrary.ValueWatchers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HsManCommonLibrary;

class Test
{
    public Dictionary<string, bool> Dict { get; set; } = new Dictionary<string, bool>();
}

public class ApiResponse<TError, TData, TResponseObject>
{
    public ApiResponse(TError? error, TData? data, TResponseObject originalResponse)
    {
        Error = error;
        Data = data;
        OriginalResponse = originalResponse;
    }

    public TError? Error { get; }
    public TData? Data { get; }
    public TResponseObject OriginalResponse { get; }

    public virtual bool IsSuccess => Error == null;
}


public class HttpApiResponse<TError, TData> : ApiResponse<TError, TData, HttpResponseMessage>
{
    public HttpApiResponse(TError? error, TData? data, HttpResponseMessage originalResponse) : 
        base(error, data, originalResponse)
    {
    }

    public override bool IsSuccess => Error != null && Data != null;
}

public static class OsuApiPaths
{
    public const string GetBeatmaps = "/api/get_beatmaps";
}


public class OsuApiRequest
{
    /// <summary>
    ///  Argument references can be found on https://github.com/ppy/osu-api/wiki
    /// </summary>
    List<KeyValuePair<string, string>> _arguments  = new List<KeyValuePair<string, string>>();
    public string ApiKey { get; set; } = "";
    public string ApiPath { get; set; } = "";

    public void AddArgument(object key, object val)
    {
        var keyString = key.ToString();
        var valString = val.ToString() ?? "";
        if (string.IsNullOrEmpty(keyString))
        {
            return;
        }
        
        _arguments.Add(new KeyValuePair<string, string>(keyString, valString));
    }

    private const string OsuUrl = "osu.ppy.sh"; 

    public string BuildUrl()
    {
        UriBuilder builder = new UriBuilder();
        builder.Host = OsuUrl;
        builder.Path = ApiPath;
        StringBuilder argumentsBuilder = new StringBuilder();
        AddArgument("k", ApiKey);
        for (int i = 0; i < _arguments.Count; i++)
        {
            argumentsBuilder.Append($"{_arguments[i].Key}={_arguments[i].Value}");
            if (i < _arguments.Count - 1)
            {
                argumentsBuilder.Append('&');
            }
        }

        builder.Query = argumentsBuilder.ToString();
        builder.Scheme = "https://";
        string builtUri = builder.ToString();
        return builtUri;
    }
}



public static class OsuApiInvoker
{
    private static HttpClient _client = new HttpClient();

    public static async Task<HttpApiResponse<JToken, JToken>> InvokeApiAsync(OsuApiRequest request)
    {
        string url = request.BuildUrl();
        var res = await _client.GetAsync(url);
        var resStr = await res.Content.ReadAsStringAsync();
        var jsonData = JsonConvert.DeserializeObject<JToken>(resStr);
        if (jsonData == null)
        {
            return new HttpApiResponse<JToken, JToken>(null, default, res);
        }

        if (jsonData is JObject jObject && jObject.ContainsKey("error"))
        {
            return new HttpApiResponse<JToken, JToken>(jsonData, default, res);
        }

       
        return new HttpApiResponse<JToken, JToken>(null, jsonData , res);
    }
}

class Beatmaps
{
    public List<JToken> Tokens { get; } = new List<JToken>();
}
class Program
{
   
    static void Main(string[] args)
    {
        /*OsuApiRequest request = new OsuApiRequest();
        request.ApiKey = "fa2748650422c84d59e0e1d5021340b6c418f62f";
        request.ApiPath = OsuApiPaths.GetBeatmaps;
        var r = OsuApiInvoker.InvokeApiAsync(request).Result;

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        for (int i = 0; i < 500; i++)
        {
            var adapted = new JsonNestedValueStoreAdapter().ToNestedValue(r.Data);
            Beatmaps beatmaps = new Beatmaps();
            ObjectAssigner.AssignTo(beatmaps, adapted, null);
        }

        stopwatch.Stop();
        Console.WriteLine(stopwatch.Elapsed);*/
        
        
    }
}