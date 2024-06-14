using HsManCommonLibrary.Pooling;

namespace HsManCommonLibrary.Locks;

public class LockerManagerPool : ObjectPool<LockManager>
{
    public static LockerManagerPool Default { get; } = new LockerManagerPool();
}