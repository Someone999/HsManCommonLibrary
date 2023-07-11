namespace HsManCommonLibrary.Timers;

public interface ITimer
{
    event TimerElapsedEventHandler Elapsed;
    double Interval { get; set; }
    void Start(bool invokeAsync);
    void Stop();
    void Reset();
    bool AutoReset { get; set; }
}