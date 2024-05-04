using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<RemoveAllBenchmarks>();

[SimpleJob, MemoryDiagnoser(false)]
public class RemoveAllBenchmarks
{
    private readonly List<string?> _list = new();

    [GlobalSetup]
    public void Setup()
    {
        for (var iterator = 0; iterator < 100; iterator++)
            _list.Add(iterator % 2 == 0 ? null : string.Empty);
    }

    [Benchmark]
    public void RemoveAll()
    {
        _list.RemoveAll(x => x is null);
    }

    [Benchmark]
    public void ForLoop()
    {
        for (var index = 0; index < _list.Count; index++)
        {
            if (_list[index] is null)
                _list.RemoveAt(index--);
        }
    }

    [Benchmark]
    public void ReverseForLoop()
    {
        for (var index = _list.Count - 1; index >= 0; index--)
        {
            if (_list[index] is null)
                _list.RemoveAt(index);
        }
    }
}
