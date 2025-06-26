using HsManCommonLibrary.Exceptions;
using HsManCommonLibrary.Reflections;
using HsManCommonLibrary.ExtraMethods;

namespace HsManCommonLibrary.NestedValues.NestedValueAdapters;

public static class NestedValueAdapterManager
{
    static NestedValueAdapterManager()
    {
        Init();
    }
    private static Dictionary<Type, INestedValueStoreAdapter>? _adapters;
    private static readonly object StaticLocker = new object();

    private static void Init()
    {
        lock (StaticLocker)
        {
            ReflectionAssemblyManager.DefaultInstance.AddAssembly(typeof(NestedValueAdapterManager).Assembly);
            if (_adapters != null)
            {
                return;
            }

            _adapters = new Dictionary<Type, INestedValueStoreAdapter>();
            var types = 
                ReflectionAssemblyManager.DefaultInstance.CreateAssemblyTypeCollection().GetSubTypesOf<INestedValueStoreAdapter>();

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

    public static INestedValueStoreAdapter? GetAdapterByAdaptableType(Type t)
    {
        if (_adapters == null)
        {
            throw new HsManInternalException("Field was failed to init");
        }

        return  _adapters.Values.FirstOrDefault(adaptersValue => adaptersValue.CanConvert(t));
    }
        
    public static INestedValueStoreAdapter? GetAdapterByAdaptableType<T>()
    {
        return GetAdapterByAdaptableType(typeof(T));
    }

    public static INestedValueStoreAdapter? GetAdapterByAdapterType(Type t)
    {
        if (_adapters == null)
        {
            throw new HsManInternalException("Field was failed to init");
        }

        return _adapters.GetValueOrDefault(t);
    }
        
    public static INestedValueStoreAdapter? GetAdapterByAdapterType<T>()
    {
        return GetAdapterByAdapterType(typeof(T));
    }

    
}