namespace HsManCommonLibrary.Timing;

public interface IControllableClock : IClock
{
    void Start();
    void Stop();
    void Reset();
}