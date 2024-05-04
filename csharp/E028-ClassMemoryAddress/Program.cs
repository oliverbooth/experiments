unsafe
{
    var instance = new MyClass();
    TypedReference instanceRef = __makeref(instance);
    IntPtr instancePtr = **(IntPtr**)(&instanceRef);

    Console.WriteLine($"{instancePtr:X}");
}

class MyClass
{
}
