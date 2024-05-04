foreach (int number in Get10RandomNumbers())
{
    Console.WriteLine(number);
}

static IEnumerable<int> Get10RandomNumbers()
{
    var random = new Random();
    for (var i = 0; i < 10; i++)
        yield return random.Next();
}
