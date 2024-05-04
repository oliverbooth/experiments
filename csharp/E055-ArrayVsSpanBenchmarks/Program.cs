using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<ArrayVsSpanBenchmarks>();

[SimpleJob, MemoryDiagnoser(false)]
public class ArrayVsSpanBenchmarks
{
    [Benchmark]
    public int Array()
    {
        var array = new int[10_000];
        return array.Length;
    }

    [Benchmark]
    public int Span()
    {
        Span<int> span = stackalloc int[10_000];
        return span.Length;
    }
}
