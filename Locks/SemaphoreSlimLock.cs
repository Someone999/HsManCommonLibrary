namespace HsManCommonLibrary.Locks;

public class SemaphoreSlimLock : ILock
{
    private readonly SemaphoreSlim _semaphoreSlim;
    
    public SemaphoreSlimLock(int initialCount = 1, int maximumCount = 1)
    {
        _semaphoreSlim = new SemaphoreSlim(initialCount, maximumCount);
        Console.WriteLine($"{initialCount} {maximumCount}");
    }
    public bool Lock()
    {
        return _semaphoreSlim.Wait(-1);
    }

    public bool Lock(int timeoutMillisecond)
    {
        return _semaphoreSlim.Wait(timeoutMillisecond);
    }

    public async Task WaitAsync(int timeoutMillisecond)
    {
        try
        {
            await _semaphoreSlim.WaitAsync(timeoutMillisecond);
        }
        catch (Exception)
        {
            // 处理异常
        }
    }

    public void Release()
    {
        _semaphoreSlim.Release();
    }
}