using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<StringBenchmarks>();

[SimpleJob, MemoryDiagnoser(false)]
public class StringBenchmarks
{
    [Benchmark]
    public string AllocEmptyString()
    {
        var chars = ReadOnlySpan<char>.Empty;
        return new string(chars);
    }
}
