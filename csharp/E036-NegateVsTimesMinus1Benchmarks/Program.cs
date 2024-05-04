using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<NegateVsTimesMinus1Benchmarks>();

[SimpleJob]
public class NegateVsTimesMinus1Benchmarks
{
    [Benchmark]
    [Arguments(1)]
    [Arguments(10)]
    [Arguments(100)]
    public float Negate(float x) => -x;

    [Benchmark]
    [Arguments(1)]
    [Arguments(10)]
    [Arguments(100)]
    public float TimesMinus1(float x) => x * -1;
}
