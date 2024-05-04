string[] foo;
bool ready = (foo = ["1", "2"]).Length <= int.MaxValue;

Console.WriteLine(ready);
Console.WriteLine(string.Join(", ", foo));
