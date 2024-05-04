unsafe
{
    A a = new();
    a.a->i = 1;
    Console.WriteLine(a.i);
    Console.WriteLine(a.a->i);

    A b = new();
    b.a->i = 1;
    Console.WriteLine(b.i);
    Console.WriteLine(b.a->i);
    b.i = 2;
}

unsafe struct A
{
    public A()
    {
        // ReSharper disable once LocalVariableHidesMember
        fixed (A* a = &this)
        {
            this.a = a;
        }

        i = 0;
    }

    public A* a;

    public int i;
}
