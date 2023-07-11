using System.Collections.Concurrent;

namespace HsManCommonLibrary.Locks;

public class LockManager
{
    private ConcurrentDictionary<string, object> _lockers = new ConcurrentDictionary<string, object>();
    private ConcurrentDictionary<string, int> _usage = new ConcurrentDictionary<string, int>();
    private ConcurrentDictionary<string, int> _zeroReferenceCounts = new ConcurrentDictionary<string, int>();

    public int MaxLockCount { get; set; } = 10;
    public int ZeroReferenceThreshold { get; set; } = 3;
    string GetLeastUsedLock()
    {
        var lockName = _usage.OrderBy(o => o.Value).ElementAt(0);
        return lockName.Key;
    }

    void CheckZeroReference()
    {
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
    }
    
    void RemoveZeroReferenceLocks()
    {
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
            if (lockName is null)
            {
                continue;
            }

            _lockers.TryRemove(lockName, out _);
            _usage.TryRemove(lockName, out _);
            _zeroReferenceCounts.TryRemove(lockName, out _);
        }
    }
    
    public object AcquireLockObject(string name)
    {
        object retObject;
        RemoveZeroReferenceLocks();
        if (!_lockers.ContainsKey(name))
        {
            if (_lockers.Count == MaxLockCount)
            {
                _lockers.TryRemove(GetLeastUsedLock(), out _);
                _usage.TryRemove(GetLeastUsedLock(), out _);
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

    public void ReleaseLock(string name)
    {
        if (!_usage.ContainsKey(name) || _usage[name] <= 0)
        {
            return;
        }

        _usage[name]--;
    }
}