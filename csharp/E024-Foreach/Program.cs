foreach (int i in new CountTo(10))
{
    Console.WriteLine(i);
}

public readonly struct CountTo(int max)
{
    public CountToEnumerator GetEnumerator() => new(max);

    public struct CountToEnumerator(int max)
    {
        public int Current { get; private set; } = 0;

        public bool MoveNext() => Current++ < max;
    }
}
