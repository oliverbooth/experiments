using System.Timers;
using Timer = System.Timers.Timer;

var timer = new Timer
{
    Interval = 1000,
    Enabled = true
};
timer.Elapsed += TimerOnElapsed;

Console.WriteLine($"Calling start in thread {Environment.CurrentManagedThreadId}");
timer.Start();

Console.ReadLine();

static void TimerOnElapsed(object? sender, ElapsedEventArgs e)
{
    (sender as Timer)?.Stop();
    Console.WriteLine($"Elapsed raised in thread {Environment.CurrentManagedThreadId}");
}
