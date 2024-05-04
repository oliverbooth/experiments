using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<ConcatVsStringBuilderBenchmarks>();

[SimpleJob, MemoryDiagnoser(false)]
public class ConcatVsStringBuilderBenchmarks
{
    private int _count;
    private string _string;

    [GlobalSetup]
    public void Setup()
    {
        _count = 100;
        _string = "Hello World";
    }

    [Benchmark]
    public string PlusOperator()
    {
        var result = "";

        for (var i = 0; i < _count; i++)
            result += _string;

        return result;
    }

    [Benchmark]
    public string StringBuilder()
    {
        var result = new StringBuilder();

        for (var i = 0; i < _count; i++)
            result.Append(_string);

        return result.ToString();
    }
}
