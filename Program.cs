using System.Diagnostics;
using HsManCommonLibrary.Logging;
using HsManCommonLibrary.Logging.Appenders;
using HsManCommonLibrary.Logging.Formatters;
using HsManCommonLibrary.Logging.Loggers;
using HsManCommonLibrary.Logging.MessageObjectProcessors;
using HsManCommonLibrary.NestedValues.NestedValueAdapters;
using HsManCommonLibrary.ValueHolders;
using YamlDotNet.Serialization;

namespace HsManCommonLibrary;

class Program
{
    
    static void Main(string[] args)
    {
        var cfg = @"L:\Other\Code\lxzqwq\bin\Debug\net6.0\configs\config.yaml";
        Deserializer deserializer = new Deserializer();
        var obj = deserializer.Deserialize<Dictionary<string, object>>(File.ReadAllText(cfg));
        var n = DictionaryNestedValueStoreAdapter.Adapter.ToNestedValue(obj);
        var x = n["Server"]?["ApiServer"]?["Ip"]?.GetValueAs<string>();
    }
}