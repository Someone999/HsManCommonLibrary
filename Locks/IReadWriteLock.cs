namespace CommonLibrary.Locks;

public interface IReadWriteLock : ILock
{
    bool LockRead();
    bool LockWrite();
    bool LockRead(int timeoutMillisecond);
    bool LockWrite(int timeoutMillisecond);
    bool ReleaseRead();
    bool ReleaseWrite();
}