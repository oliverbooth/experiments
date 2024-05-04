using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<PowVsManualSquare>();

[SimpleJob]
public class PowVsManualSquare
{
    [Benchmark]
    [Arguments(1, 2)]
    [Arguments(10, 3)]
    [Arguments(100, 4)]
    public double Pow(double x, int y)
    {
        return Math.Pow(x, 2);
    }

    [Benchmark]
    [Arguments(1, 2)]
    [Arguments(10, 3)]
    [Arguments(100, 4)]
    public double ForLoop(double x, int y)
    {
        var result = 1.0;

        for (var i = 0; i < y; i++)
            result *= x;

        return result;
    }

    [Benchmark]
    [Arguments(1)]
    [Arguments(10)]
    [Arguments(100)]
    public double XTimesX(double x)
    {
        return x * x;
    }
}
