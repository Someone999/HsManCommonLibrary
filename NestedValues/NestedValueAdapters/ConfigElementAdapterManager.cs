using HsManCommonLibrary.Reflections;

namespace HsManCommonLibrary.NestedValues.NestedValueAdapters;

public static class ConfigElementAdapterManager
{
    static ConfigElementAdapterManager()
    {
        Init();
    }
    private static Dictionary<Type, INestedValueStoreAdapter>? _adapters;
    private static readonly object StaticLocker = new object();

    private static void Init()
    {
        lock (StaticLocker)
        {
            if (_adapters != null)
            {
                return;
            }

            _adapters = new Dictionary<Type, INestedValueStoreAdapter>();
            var types = 
                ReflectionAssemblyManager.CreateAssemblyTypeCollection().GetSubTypesOf<INestedValueStoreAdapter>();

            foreach (var type in types)
            {
                var ins = type.GetConstructor(Type.EmptyTypes)?.Invoke(null);
                if (ins == null)
                {
                    continue;
                }
                
                _adapters.Add(type, (INestedValueStoreAdapter) ins);
            }
        }
            
    }

    public static INestedValueStoreAdapter GetAdapterByAdaptableType(Type t)
    {
        return _adapters?.Values.FirstOrDefault(adaptersValue => adaptersValue.CanConvert(t)) 
               ?? throw new KeyNotFoundException();
    }
        
    public static INestedValueStoreAdapter GetAdapterByAdaptableType<T>()
    {
        return GetAdapterByAdaptableType(typeof(T));
    }

    public static INestedValueStoreAdapter GetAdapterByAdapterType(Type t)
    {
        return _adapters?[t] ?? throw new KeyNotFoundException();
    }
        
    public static INestedValueStoreAdapter GetAdapterByAdapterType<T>()
    {
        return GetAdapterByAdaptableType(typeof(T));
    }
}