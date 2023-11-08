using System.Collections.Concurrent;

namespace HsManCommonLibrary.Locks;

public class LockManager
{
    private readonly ConcurrentDictionary<string, object> _lockers = new ConcurrentDictionary<string, object>();
    private readonly ConcurrentDictionary<string, int> _usage = new ConcurrentDictionary<string, int>();
    private readonly ConcurrentDictionary<string, int> _zeroReferenceCounts = new ConcurrentDictionary<string, int>();

    private readonly ReaderWriterLockSlim _removeZeroRefLockReadWriterLockSlim = new ReaderWriterLockSlim();
   
    public int MaxLockCount { get; set; } = 1024;
    public int ZeroReferenceThreshold { get; set; } = 10;

    string? GetLeastUsedLock()
    {
        if (_usage.IsEmpty)
        {
            return null;
        }
        
        var lockName = _usage.OrderBy(o => o.Value).ElementAt(0);
        return lockName.Key;
    }

    void CheckZeroReference()
    {
        _removeZeroRefLockReadWriterLockSlim.EnterWriteLock();
        foreach (var reference in _usage)
        {
            if (reference.Value != 0)
            {
                continue;
            }

            if (!_zeroReferenceCounts.ContainsKey(reference.Key))
            {
                _zeroReferenceCounts.TryAdd(reference.Key, 1);
            }
            else
            {
                _zeroReferenceCounts[reference.Key]++;
            }
        }
        _removeZeroRefLockReadWriterLockSlim.ExitWriteLock();
    }

    public void RemoveZeroReferenceLocks()
    {
        _removeZeroRefLockReadWriterLockSlim.EnterWriteLock();
        CheckZeroReference();
        ConcurrentBag<string> bag = new ConcurrentBag<string>();
        foreach (var reference in _zeroReferenceCounts)
        {
            if (reference.Value > ZeroReferenceThreshold)
            {
                bag.Add(reference.Key);
            }
        }

        foreach (var lockName in bag)
        {
            _lockers.TryRemove(lockName, out _);
            _usage.TryRemove(lockName, out _);
            _zeroReferenceCounts.TryRemove(lockName, out _);
        }

        _removeZeroRefLockReadWriterLockSlim.ExitWriteLock();
    }

    public object AcquireLockObject(string name)
    {
        object retObject;
        if (!_lockers.ContainsKey(name))
        {
            if (_lockers.Count == MaxLockCount)
            {
                string? leastUsedLock = GetLeastUsedLock();
                if (leastUsedLock != null)
                {
                    _lockers.TryRemove(leastUsedLock, out _);
                    _usage.TryRemove(leastUsedLock, out _);
                }
            }

            _lockers.TryAdd(name, retObject = new object());
            _usage.TryAdd(name, 1);
        }
        else
        {
            _usage[name]++;
            retObject = _lockers[name];
        }

        return retObject;
    }
    
    
}