namespace HsManCommonLibrary.Common;

public interface IRetryPolicy
{
    public int MaxRetries { get; set; }
    public int RetryDelay { get; set; }
    public bool EnableIncrementer { get; set; }
    public IRetryDelayIncrementer? RetryDelayIncrementer { get; set; }
    public int MaxRetryDelay { get; set; }
    public void StartRetry(object? context);
    public void StopRetry();
    event Action<int, Exception>? Retry;
}