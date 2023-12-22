using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using HsManCommonLibrary.Cache;
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

class Program
{
   
    static void Main(string[] args)
    {
        NameStyleDetector.AddNameStyle(new UpperCamelCaseNameStyle());
        NameStyleDetector.AddNameStyle(new LowerCamelCaseNameStyle());
        string json = "{\"dict\": {\"a\": 5, \"b\": 7}}";
        var adapted = new JsonNestedValueStoreAdapter()
            .ToNestedValue(JsonConvert.DeserializeObject<JToken>(json));

        Test t = new Test();
        ObjectAssigner.AssignTo(t, adapted, new AssignOptions());
        adapted = adapted["dict"];
        if (adapted == null)
        {
            throw new Exception("Fuck you!");
        }

        
        ;
    }
}