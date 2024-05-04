using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<BigOLoop>();

[SimpleJob, MemoryDiagnoser(false)]
public class BigOLoop
{
    [Benchmark]
    [Arguments(1)]
    [Arguments(10)]
    [Arguments(100)]
    [Arguments(1000)]
    [Arguments(10000)]
    public void Linear(int dimension)
    {
        int total = dimension * dimension;
        for (var iterator = 0; iterator < total; iterator++)
        {
        }
    }

    [Benchmark]
    [Arguments(1)]
    [Arguments(10)]
    [Arguments(100)]
    [Arguments(1000)]
    [Arguments(10000)]
    public void Exponential(int dimension)
    {
        for (var x = 0; x < dimension; x++)
        for (var y = 0; y < dimension; y++)
        {
        }
    }
}
