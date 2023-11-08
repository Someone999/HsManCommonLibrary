using HsManCommonLibrary.Collections.Transactional.Transactions;
using HsManCommonLibrary.Collections.Transactional.Transactions.TransactionOperations;
using HsManCommonLibrary.CommandLine;
using HsManCommonLibrary.Configuration;
using HsManCommonLibrary.NestedValues.JsonLikeFormats;
using HsManCommonLibrary.NestedValues.NestedValueAdapters;
using HsManCommonLibrary.NestedValues.NestedValueConverters;
using Newtonsoft.Json;
using YamlDotNet.Serialization;

namespace HsManCommonLibrary;

class Program
{
   
    static void Main(string[] args)
    {
        CommandLineParser parser = new CommandLineParser();
        var x = parser.Parse("program -c config.json -t json -exe rust_server.exe --executable rustdedicated.exe");
        
    }
}