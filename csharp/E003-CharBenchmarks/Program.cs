using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<CharBenchmark>();

[SimpleJob, MemoryDiagnoser(false)]
public class CharBenchmark
{
    [Benchmark]
    public char TestA()
    {
        return 'a';
    }

    [Benchmark]
    public char TestUtf16()
    {
        return '\u0369';
    }
}
