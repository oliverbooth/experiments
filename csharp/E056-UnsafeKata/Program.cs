using System.Runtime.CompilerServices;

var t = 424;
byte[] bt = Unsafe.As<int, byte[]>(ref t);

bool a = Unsafe.IsNullRef(ref bt);
bool b = bt is null;

Console.WriteLine(a);
Console.WriteLine(b);

Console.WriteLine(bt);
