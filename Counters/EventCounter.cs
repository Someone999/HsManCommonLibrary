using System.Diagnostics;

namespace HsManCommonLibrary.Counters;

public class EventCounter
{
    private Stopwatch _stopwatch = new Stopwatch();
    private int _counter;

    /// <summary>
    /// Time limit in second
    /// </summary>
    public int TimeLimit { get; set; } = 3;

    public int CountLimit { get; set; } = 5;

    public void Start()
    {
        _stopwatch.Start();
    }

    public event EventHandler? OnOverFrequency;
    
    public void TriggerEvent()
    {
        if (_stopwatch.ElapsedMilliseconds / 1000.0 >= TimeLimit)
        {
            _counter = 0;
            _stopwatch.Reset();
        }
        else
        {
            _counter++;
            if (_counter <= CountLimit || OnOverFrequency == null)
            {
                return;
            }
            
            _counter = 0;
            _stopwatch.Reset();
            OnOverFrequency.Invoke(this, EventArgs.Empty);
        }
    }
}