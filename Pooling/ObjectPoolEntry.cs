namespace HsManCommonLibrary.Pooling;

public class ObjectPoolEntry<T>
{
    public ObjectPoolEntry(T pooledObject)
    {
        PooledObject = pooledObject;
    }

    public T PooledObject { get; }
    public bool IsUsing { get; private set; }
    internal void AcquireObject() => IsUsing = true;
    internal void ReturnObject() => IsUsing = false;
}