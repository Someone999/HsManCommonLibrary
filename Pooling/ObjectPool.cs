namespace HsManCommonLibrary.Pooling;

public class ObjectPool<T> where T : notnull
{
    private Dictionary<T, ObjectPoolEntry<T>> _entries = new Dictionary<T, ObjectPoolEntry<T>>();
    public int MaxSize { get; set; } = 32;
    private readonly object _readWriteLock = new object();

    public ObjectPoolEntry<T> AddToPool(ObjectPoolEntry<T> entry)
    {
        _entries.Add(entry.PooledObject, entry);
        return entry;
    }

    ObjectPoolEntry<T> AcquireObjectAwait()
    {
        Monitor.Wait(_readWriteLock);
        return AcquireObject();
    }

    protected virtual T CreateNewObject()
    {
        T nObj = Activator.CreateInstance<T>();
        if (nObj == null)
        {
            throw new Exception($"Can not instant the type {typeof(T)}");
        }

        return nObj;
    }

    public ObjectPoolEntry<T> AcquireObject()
    {
        lock (_readWriteLock)
        {
            var firstFreeEntry =
                _entries.FirstOrDefault(o => !o.Value.IsUsing);

            if (firstFreeEntry.Value != null)
            {
                firstFreeEntry.Value.AcquireObject();
                return firstFreeEntry.Value;
            }


            if (_entries.Count >= MaxSize)
            {
                return AcquireObjectAwait();
            }

            var nObj = CreateNewObject();
            var entry = new ObjectPoolEntry<T>(nObj);
            _entries.Add(nObj, entry);
            return entry;
        }
    }

    public bool ReturnObject(T obj)
    {
        lock (_readWriteLock)
        {
            if (!_entries.TryGetValue(obj, out var entry))
            {
                return false;
            }

            entry.ReturnObject();
            Monitor.Pulse(_readWriteLock);
            return !entry.IsUsing;
        }
    }
}