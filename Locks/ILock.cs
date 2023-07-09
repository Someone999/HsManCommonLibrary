using System.Diagnostics;

namespace CommonLibrary.Locks;

public interface ILock
{
    bool Lock();
    bool Lock(int timeoutMillisecond);
    void Release();
}