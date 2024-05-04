IEnumerable<int> foo = new int[50].Select(i =>
{
    Console.WriteLine("Yikes");
    return i;
});

var count = 0;
foreach (int a in foo)
{
    count++;
    if (count == 5)
    {
        break;
    }
}
