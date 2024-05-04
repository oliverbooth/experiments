using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<IntegerToDecimalBenchmarks>();

[SimpleJob, MemoryDiagnoser(false)]
public class IntegerToDecimalBenchmarks
{
    private const int Value = 69_420; // nice, and blaze it

    [Benchmark]
    [Arguments(Value)]
    public float UsingToStringAndParse(int value)
    {
        return float.Parse($"0.{value}");
    }

    [Benchmark]
    [Arguments(Value)]
    public float UsingDivide(int value)
    {
        if (value == 0)
        {
            return 0;
        }

        return value / MathF.Pow(10, MathF.Floor(MathF.Log10(MathF.Abs(value))) + 1);
    }
}
