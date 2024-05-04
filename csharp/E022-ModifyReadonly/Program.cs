var foo = new Foo(42);
Console.WriteLine(foo.X);
foo.ChangeX(69);
Console.WriteLine(foo.X);

public struct Foo
{
    public readonly int X;

    public Foo(int x)
    {
        X = x;
    }

    public readonly unsafe void ChangeX(int newValue)
    {
        fixed (int* ptr = &X)
        {
            *ptr = newValue;
        }
    }
}
