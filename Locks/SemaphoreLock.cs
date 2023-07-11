namespace HsManCommonLibrary.Locks;

public class SemaphoreLock : IMutexLock
{
    private readonly Semaphore _semaphore;

    public SemaphoreLock(int initialCount = 1, int maximumCount = 1, string? name = null)
    {

        _semaphore = name == null
            ? new Semaphore(initialCount, maximumCount)
            : new Semaphore(initialCount, maximumCount, name);

    }
    public bool Lock()
    {
        return _semaphore.WaitOne();
    }

    public bool Lock(int timeoutMillisecond)
    {
        return _semaphore.WaitOne(timeoutMillisecond);
    }

    public void Release()
    {
        _semaphore.Release();
    }
}