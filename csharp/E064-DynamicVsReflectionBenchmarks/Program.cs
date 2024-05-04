using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<DynamicVsReflectionBenchmarks>();

[SimpleJob, MemoryDiagnoser(false)]
public class DynamicVsReflectionBenchmarks
{
    [Benchmark]
    public int Normal()
    {
        var s = "Hello World";
        return s.Length;
    }

    [Benchmark]
    public int Dynamic()
    {
        dynamic s = "Hello World";
        return s.Length;
    }

    [Benchmark]
    public int Reflection()
    {
        var s = "Hello World";
        return (int)typeof(string).GetProperty("Length").GetValue(s);
    }
}
