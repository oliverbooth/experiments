using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<LinqBenchmarks>();

[SimpleJob, MemoryDiagnoser(false)]
public class LinqBenchmarks
{
    private readonly List<int> _list = Enumerable.Range(1, 1000000).ToList();

    [Benchmark]
    public int[] CustomLoop()
    {
        var array = new int[_list.Count];
        var resultIndex = 0;

        for (var index = 0; index < _list.Count; index++)
        {
            if (_list[index] % 2 == 0)
            {
                array[resultIndex++] = _list[index];
            }
        }

        Array.Resize(ref array, resultIndex);
        Array.Sort(array, (a, b) => b - a);
        return array;
    }

    [Benchmark]
    public int[] Where_OrderByDescending()
    {
        return _list.Where(i => i % 2 == 0).OrderByDescending(i => i).ToArray();
    }
}
