var total = 0;

for (var a = 1; a <= 98; a++)
for (var b = 1; b <= 98; b++)
for (var c = 1; c <= 98; c++)
{
    Console.Title = $"{++total:N0} completed";

    if (a <= 2 * b) continue;
    if (3 * b <= 4 * c) continue;
    if (3 * c <= a) continue;

    if (a + b + c == 100)
        Console.WriteLine($"Found {a} + {b} + {c} = 100");
}

Console.WriteLine("All combinations exhausted");
Console.ReadLine();
