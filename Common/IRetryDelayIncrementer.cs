namespace HsManCommonLibrary.Common;

public interface IRetryDelayIncrementer
{
    int IncrementTimer(IRetryPolicy retryPolicy);
}