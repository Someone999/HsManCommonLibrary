using System.Collections.Concurrent;

namespace HsManCommonLibrary.Locks;

public class LockManager
{
    private readonly Dictionary<string, object> _lockers = new Dictionary<string, object>();
    private readonly Dictionary<string, int> _usage = new Dictionary<string, int>();
    private readonly Dictionary<string, int> _zeroReferenceCounts = new Dictionary<string, int>();

    private readonly ReaderWriterLockSlim _removeZeroRefLockReadWriterLockSlim = new ReaderWriterLockSlim();
   
    public int MaxLockCount { get; set; } = 1024;
    public int ZeroReferenceThreshold { get; set; } = 10;

    string? GetLeastUsedLock()
    {
        if (_usage.Count == 0)
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
                _zeroReferenceCounts.Add(reference.Key, 1);
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
            _lockers.Remove(lockName);
            _usage.Remove(lockName);
            _zeroReferenceCounts.Remove(lockName);
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
                    _lockers.Remove(leastUsedLock);
                    _usage.Remove(leastUsedLock);
                }
            }

            _lockers.Add(name, retObject = new object());
            _usage.Add(name, 1);
        }
        else
        {
            _usage[name]++;
            retObject = _lockers[name];
        }

        return retObject;
    }
}