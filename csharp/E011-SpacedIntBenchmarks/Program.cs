using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<SpacedIntBenchmarks>();

[SimpleJob, MemoryDiagnoser(false)]
public class SpacedIntBenchmarks
{
    private const int Value = 12345689;

    [Benchmark]
    [Arguments(Value)]
    public string SelectWithConcat(int value)
    {
        return string.Concat(value.ToString().Select(digit => $"{digit} "));
    }

    [Benchmark]
    [Arguments(Value)]
    public string DivisionWithBuilder(int value)
    {
        var builder = new StringBuilder();

        while (value > 0)
        {
            int digit = value - (value / 10 * 10);
            builder.Insert(0, digit);
            builder.Insert(1, ' ');

            value /= 10;
        }

        return builder.ToString();
    }
}
