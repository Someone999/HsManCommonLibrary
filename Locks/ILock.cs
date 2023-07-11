namespace HsManCommonLibrary.Locks;

public interface ILock
{
    bool Lock();
    bool Lock(int timeoutMillisecond);
    void Release();
}