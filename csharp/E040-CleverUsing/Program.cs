Console.WriteLine("This outputs in default color");
using (new DisposableConsoleColor(ConsoleColor.Red))
{
    Console.WriteLine("This outputs in red");
}

Console.WriteLine("This once again outputs in default color");

public class DisposableConsoleColor : IDisposable
{
    private readonly ConsoleColor _oldColor;

    public DisposableConsoleColor(ConsoleColor color)
    {
        _oldColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
    }

    public void Dispose()
    {
        Console.ForegroundColor = _oldColor;
    }
}
