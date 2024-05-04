using System.Runtime.InteropServices;

unsafe
{
    var i = (int*)Marshal.AllocHGlobal(sizeof(int)); // 4 bytes on heap
    *i = 123; // deref and assign
    Console.WriteLine(*i); // deref and print
    Marshal.FreeHGlobal(new nint(i));

    const string str = "Hello";
    fixed (char* ptr = str)
    {
        *(ptr + 1) = 'a';
    }

    Console.WriteLine("Hello");
    Console.WriteLine("Hello" == "Hallo");
    Console.WriteLine("Hello".Equals("Hallo"));
    Console.WriteLine(ReferenceEquals("Hello", str));
}
