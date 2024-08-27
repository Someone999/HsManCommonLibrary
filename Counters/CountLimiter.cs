using System;
using System.Diagnostics;

namespace HsManCommonLibrary.Counters
{
    public class CountLimiter
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private int _counter;
        private readonly object _lock = new object();

        /// <summary>
        /// Gets or sets the time limit in seconds.
        /// </summary>
        public int TimeLimit { get; set; } = 3;

        /// <summary>
        /// Gets or sets the count limit.
        /// </summary>
        public int CountLimit { get; set; } = 5;

        /// <summary>
        /// Occurs when the frequency exceeds the count limit within the time limit.
        /// </summary>
        public event EventHandler? FrequencyExceeded;

        /// <summary>
        /// Starts the stopwatch for counting events.
        /// </summary>
        public void Start()
        {
            lock (_lock)
            {
                if (!_stopwatch.IsRunning)
                {
                    _stopwatch.Start();
                }
            }
        }

        /// <summary>
        /// Triggers an event and checks if the frequency exceeds the set limits.
        /// </summary>
        public void TriggerEvent()
        {
            lock (_lock)
            {
                if (_stopwatch.ElapsedMilliseconds / 1000.0 >= TimeLimit)
                {
                    ResetCounter();
                }

                _counter++;

                if (_counter > CountLimit)
                {
                    FrequencyExceeded?.Invoke(this, EventArgs.Empty);
                    ResetCounter();
                }
            }
        }

        /// <summary>
        /// Resets the event counter and stopwatch.
        /// </summary>
        private void ResetCounter()
        {
            _counter = 0;
            _stopwatch.Restart();
        }
    }
}