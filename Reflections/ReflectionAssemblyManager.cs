using System.Reflection;

namespace HsManCommonLibrary.Reflections;

public static class ReflectionAssemblyManager
{
    private static List<Assembly> _registeredAssemblies = new List<Assembly>();
    private static readonly object StaticLocker = new object();

    public static void AddAssembly(Assembly assembly)
    {
        lock (StaticLocker)
        {
            if (!_registeredAssemblies.Contains(assembly))
            {
                _registeredAssemblies.Add(assembly);
            }
        }
    }

    public static void AddAssemblyFrom(string path)
    {
        try
        {
            var asm = Assembly.LoadFrom(path);
            AddAssembly(asm);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error when load assembly: {e.Message}");
        }
    }

    public static void AddAssembliesFromPath(string path, bool excludeExeFiles)
    {
        try
        {
            string[] assemblyFiles = Directory.GetFiles(path, "*.dll", SearchOption.TopDirectoryOnly);

            if (excludeExeFiles)
            {
                var tmpAssemblyFiles = Directory.GetFiles(path, "*.exe", SearchOption.TopDirectoryOnly);
                assemblyFiles = assemblyFiles.Concat(tmpAssemblyFiles).ToArray();
            }

            foreach (var assemblyFile in assemblyFiles)
            {
                AddAssemblyFrom(assemblyFile);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static AssemblyTypeCollection CreateAssemblyTypeCollection()
    {
        var asmTypes = _registeredAssemblies.Select(asm => asm.GetTypes());
        List<Type> types = new List<Type>();
        foreach (var asmType in asmTypes)
        {
            types.AddRange(asmType);
        }

        return new AssemblyTypeCollection(types);
    }
}