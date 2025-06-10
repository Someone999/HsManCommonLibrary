using System.Collections;
using HsManCommonLibrary.ValueHolders;

namespace HsManCommonLibrary.Enumerators;

public abstract class SyncEnumerator<T, TSync> : IEnumerator<T>
{
    protected IValueHolder<TSync> SyncFlags;

    protected SyncEnumerator(IValueHolder<TSync> syncFlags)
    {
       SyncFlags = syncFlags;
    }

    public abstract bool MoveNext();
    public abstract void Reset();
    public abstract T Current { get; }
    object? IEnumerator.Current => Current;
    public abstract void Dispose();
}