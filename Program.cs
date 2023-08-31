using System.Text;
using HsManCommonLibrary.NestedValues;
using HsManCommonLibrary.NestedValues.NestedValueAdapters;
using HsManCommonLibrary.Reflections;
using HsManCommonLibrary.Timers;

namespace HsManCommonLibrary;

class Program
{
   
    static void Main(string[] args)
    {
        ReflectionAssemblyManager.AddAssembly(typeof(Program).Assembly);
        JsonConfigElement jsonConfigElement = new JsonConfigElement("L:\\gc 3.6\\config.json");
        jsonConfigElement["server"].SetValue("runMode", new CommonNestedValueStore("DISPATCH"));
        var x = jsonConfigElement.ToDictionary();
    }
}