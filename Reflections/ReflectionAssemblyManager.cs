using System.Reflection;

namespace HsManCommonLibrary.Reflections;

public class ReflectionAssemblyManager
{
    private readonly List<Assembly> _registeredAssemblies = new List<Assembly>();
    private readonly object _locker = new object();

    public void AddAssembly(Assembly assembly)
    {
        lock (_locker)
        {
            if (!_registeredAssemblies.Contains(assembly))
            {
                _registeredAssemblies.Add(assembly);
            }
        }
    }

    public void AddAssemblyFrom(string path)
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

    public void AddAssembliesFromPath(string path, bool excludeExeFiles)
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

    public TypeCollection CreateAssemblyTypeCollection()
    {
        lock (_locker)
        {
            var asmTypes = _registeredAssemblies.Select(asm => asm.GetTypes());
            List<Type> types = new List<Type>();
            foreach (var asmType in asmTypes)
            {
                types.AddRange(asmType);
            }

            return new TypeCollection(types);
        }
    }

    private static readonly Lazy<ReflectionAssemblyManager> Lazy = new();
    public static ReflectionAssemblyManager DefaultInstance => Lazy.Value;
}