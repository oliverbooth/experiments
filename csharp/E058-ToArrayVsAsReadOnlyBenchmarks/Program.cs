using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<ToArrayVsAsReadOnlyBenchmarks>();

[SimpleJob, MemoryDiagnoser(false)]
public class ToArrayVsAsReadOnlyBenchmarks
{
    private readonly List<int> _100List = new(100);
    private readonly List<int> _1000List = new(1000);
    private readonly List<int> _10000List = new(10000);

    [GlobalSetup]
    public void Setup()
    {
        for (var i = 0; i < 10000; i++)
        {
            if (i < 100) _100List.Add(i);
            if (i < 1000) _1000List.Add(i);
            _10000List.Add(i);
        }
    }

    [Benchmark]
    public IReadOnlyCollection<int> AsReadOnly_100Items()
    {
        return _100List.AsReadOnly();
    }

    [Benchmark]
    public IReadOnlyCollection<int> ToArray_100Items()
    {
        return _100List.ToArray();
    }

    [Benchmark]
    public IReadOnlyCollection<int> AsReadOnly_1000Items()
    {
        return _1000List.AsReadOnly();
    }

    [Benchmark]
    public IReadOnlyCollection<int> ToArray_1000Items()
    {
        return _1000List.ToArray();
    }

    [Benchmark]
    public IReadOnlyCollection<int> AsReadOnly_10000Items()
    {
        return _10000List.AsReadOnly();
    }

    [Benchmark]
    public IReadOnlyCollection<int> ToArray_10000Items()
    {
        return _10000List.ToArray();
    }
}
