using System.Reflection;
using HsManCommonLibrary.CommandLine.Matchers;
using HsManCommonLibrary.CommandLine.Parsers;
using HsManCommonLibrary.Configuration;
using HsManCommonLibrary.NestedValues.NestedValueAdapters;
using HsManCommonLibrary.NestedValues.SaveStrategies;
using HsManCommonLibrary.Reflections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HsManCommonLibrary;

class Program
{
   
    static void Main(string[] args)
    {
        TypeWrapper wrapper = new TypeWrapper(typeof(MethodFindOptions));
        MethodFindOptions options = new MethodFindOptions();
        options.MemberName = "aaaa";
        wrapper.SetMemberValue(new MethodFindOptions()
        {
            MemberName = "MemberName",
        }, options, "bbbb");

        
    }
}