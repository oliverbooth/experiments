using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<SwapBenchmarks>();

[SimpleJob, MemoryDiagnoser(false)]
public class SwapBenchmarks
{
    [Benchmark]
    [Arguments(69, 420)]
    public void Swap1(ref int a, ref int b)
    {
        a ^= b ^ (b = a);
    }

    [Benchmark]
    [Arguments(69, 420)]
    public void Swap2(ref int a, ref int b)
    {
        a = (a ^= b) ^ (b ^= a);
    }

    [Benchmark]
    [Arguments(69, 420)]
    public void Swap3(ref int a, ref int b)
    {
        a ^= b;
        b ^= a;
        a ^= b;
    }

    [Benchmark]
    [Arguments(69, 420)]
    public void SwapClassic(ref int a, ref int b)
    {
        int t = a;
        a = b;
        b = t;
    }

    [Benchmark]
    [Arguments(69, 420)]
    public void SwapViaDeconstruction(ref int a, ref int b)
    {
        (a, b) = (b, a);
    }
}
