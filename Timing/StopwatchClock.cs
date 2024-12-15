using System.Diagnostics;

namespace HsManCommonLibrary.Timing;

public class StopwatchClock : IControllableClock
{
    private readonly Stopwatch _stopwatch = new Stopwatch();
    public TimeSpan ElapsedTime => _stopwatch.Elapsed;
    public double Rate => 1.0;

    public void Start()
    {
        _stopwatch.Start();
    }

    public void Stop()
    {
        _stopwatch.Stop();
    }

    public void Reset()
    {
        _stopwatch.Reset();
    }
}