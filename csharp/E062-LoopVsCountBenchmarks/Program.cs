using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<LoopVsCountBenchmarks>();

[SimpleJob, MemoryDiagnoser(false)]
public class LoopVsCountBenchmarks
{
    private static readonly Random Random = new();
    private readonly bool[] _array = Enumerable.Range(1, 1000000).Select(_ => Random.NextDouble() > 0.5).ToArray();

    [Benchmark]
    public int Loop()
    {
        var count = 0;

        foreach (bool item in _array)
        {
            if (item)
            {
                count++;
            }
        }

        return count;
    }

    [Benchmark]
    public int Count()
    {
        return _array.Count(item => item);
    }
}
