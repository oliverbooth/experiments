using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<DictionaryBenchmarks>();

[SimpleJob]
public class DictionaryBenchmarks
{
    public IEnumerable<int> Sizes => new[] { 10, 100, 1000, 10000, 100000, 1000000 };

    [Benchmark]
    [ArgumentsSource(nameof(Sizes))]
    public void Get(int size)
    {
        var dict = new Dictionary<int, int>();
        for (var i = 0; i < size; i++)
        {
            dict[i] = i;
        }

        var n = 0;
        for (var i = 0; i < size; i++)
        {
            n = dict[i];
        }
    }
}
