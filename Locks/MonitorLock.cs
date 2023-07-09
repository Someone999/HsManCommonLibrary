namespace CommonLibrary.Locks;

public class MonitorLock : IMutexLock
{
    private readonly object _locker = new object();
    public bool Lock()
    {
        Monitor.Enter(_locker);
        return Monitor.IsEntered(_locker);
    }

    public bool Lock(int timeoutMillisecond)
    {
        return Monitor.TryEnter(_locker, timeoutMillisecond);
    }

    public void Release()
    {
        Monitor.Exit(_locker);
    }
}