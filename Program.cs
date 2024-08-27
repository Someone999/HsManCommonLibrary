using System.Diagnostics;
using System.Reflection;
using HsManCommonLibrary.Logging;
using HsManCommonLibrary.Logging.Appenders;
using HsManCommonLibrary.Logging.Formatters;
using HsManCommonLibrary.Logging.Loggers;
using HsManCommonLibrary.Logging.MessageObjectProcessors;
using HsManCommonLibrary.NestedValues.NestedValueAdapters;
using HsManCommonLibrary.Reflections;
using HsManCommonLibrary.Reflections.Accessors;
using HsManCommonLibrary.Reflections.Finders;
using HsManCommonLibrary.ValueHolders;
using YamlDotNet.Serialization;

namespace HsManCommonLibrary;

class Program
{
    public static int Fuck { get; } = 0;
    static void Main(string[] args)
    {
        MemberFinder<PropertyInfo> finder = new MemberFinder<PropertyInfo>(typeof(Program));
        IMemberAccessor accessor = new PropertyMemberAccessor(finder.FindMember("Fuck") ?? throw new Exception());
        accessor.SetValue(null, 1);
    }
}