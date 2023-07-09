using CommonLibrary.Timers;

namespace CommonLibrary;

class Program
{
    static void Main(string[] args)
    {
        CountdownTimer timer = new CountdownTimer();
        int i = 0;
        timer.Elapsed += (sender, eventArgs) =>
        {
            if (i == 500)
            {
                eventArgs.IgnoreNextTime = true;
            }

            i++;
            //Console.WriteLine("Triggered");
        };
        timer.Interval = 10;
        timer.AutoReset = true;
        timer.Start(true);
        
        while (true) ;
    }
}