new DerivedClass(42);

public abstract class BaseClass
{
    protected BaseClass() => DoSomething();
    protected BaseClass(int x) => DoSomething();

    public abstract void DoSomething();
}

public sealed class DerivedClass : BaseClass
{
    private readonly int _someInt = 10;

    public DerivedClass(int x) => _someInt = x;

    public override void DoSomething() => Console.WriteLine(_someInt);
}
