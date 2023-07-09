using System.Data;
using System.Diagnostics;
using System.Net;

namespace CommonLibrary.Timers;

public class CountdownTimer : ITimer
{
    public event TimerElapsedEventHandler? Elapsed;
    public double Interval { get; set; }
    private Stopwatch _stopwatch = new Stopwatch();
    private bool _resetRequired;
    private bool _stopRequired;
    private bool _isRunning;
    private TimerElapsedEventArgs? _lastArgs;

    public void Start(bool invokeAsync)
    {
        if (_isRunning)
        {
            return;
        }

        _stopwatch.Restart();
        _isRunning = true;
        while (true)
        {
            if (_resetRequired)
            {
                _resetRequired = false;
                _stopwatch.Reset();
            }

            if (_stopRequired)
            {
                _stopRequired = false;
                return;
            }

            bool hasSubscriber;
            lock (this)
            {
                hasSubscriber = Elapsed != null;
            }

            if (hasSubscriber && _stopwatch.ElapsedMilliseconds > Interval)
            {
                TimerElapsedEventArgs eventArgs = new();
                if (_lastArgs is { IgnoreNextTime: true })
                {
                    _lastArgs = eventArgs;
                    continue;
                }

                if (invokeAsync)
                {
                    Task.Run(() => Elapsed!.Invoke(this, eventArgs));
                }
                else
                {
                    Elapsed!.Invoke(this, eventArgs);
                }
                
                _stopwatch.Restart();
                _lastArgs = eventArgs;
            }
            else
            {
                Thread.Sleep(1);
            }

            if (!AutoReset)
            {
                break;
            }
        }
    }

    public void Stop()
    {
        _stopRequired = true;
        _stopwatch.Stop();
        _isRunning = false;
    }

    public void Reset()
    {
        _resetRequired = true;
    }

    public bool AutoReset { get; set; }
}