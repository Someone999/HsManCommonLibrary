using HsManCommonLibrary.Reflections;

namespace HsManCommonLibrary.ConfigElements.ConfigAdapters;

public static class ConfigElementAdapterManager
{
    static ConfigElementAdapterManager()
    {
        Init();
    }
    private static Dictionary<Type, IConfigElementAdapter>? _adapters;
    private static readonly object StaticLocker = new object();

    private static void Init()
    {
        lock (StaticLocker)
        {
            if (_adapters != null)
            {
                return;
            }

            _adapters = new Dictionary<Type, IConfigElementAdapter>();
            var types = 
                ReflectionAssemblyManager.CreateAssemblyTypeCollection().GetSubTypesOf<IConfigElementAdapter>();

            foreach (var type in types)
            {
                var ins = type.GetConstructor(Type.EmptyTypes)?.Invoke(null);
                if (ins == null)
                {
                    continue;
                }
                
                _adapters.Add(type, (IConfigElementAdapter) ins);
            }
        }
            
    }

    public static IConfigElementAdapter GetAdapterByAdaptableType(Type t)
    {
        return _adapters?.Values.FirstOrDefault(adaptersValue => adaptersValue.CanConvert(t)) 
               ?? throw new KeyNotFoundException();
    }
        
    public static IConfigElementAdapter GetAdapterByAdaptableType<T>()
    {
        return GetAdapterByAdaptableType(typeof(T));
    }

    public static IConfigElementAdapter GetAdapterByAdapterType(Type t)
    {
        return _adapters?[t] ?? throw new KeyNotFoundException();
    }
        
    public static IConfigElementAdapter GetAdapterByAdapterType<T>()
    {
        return GetAdapterByAdaptableType(typeof(T));
    }
}