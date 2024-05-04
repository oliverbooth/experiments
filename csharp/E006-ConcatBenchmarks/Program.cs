using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<ConcatBenchmarks>();

[SimpleJob, MemoryDiagnoser(false)]
public class ConcatBenchmarks
{
    private string _string1;
    private string _string2;
    private string _string3;

    [GlobalSetup]
    public void Setup()
    {
        _string1 = "Hello";
        _string2 = "World";
        _string3 = "!";
    }

    [Benchmark]
    public string PlusOperator()
    {
        return _string1 + _string2 + _string3;
    }

    [Benchmark]
    public string String_Concat()
    {
        return string.Concat(_string1, _string2, _string3);
    }

    [Benchmark]
    public string StringBuilder()
    {
        return new StringBuilder().Append(_string1).Append(_string2).Append(_string3).ToString();
    }

    [Benchmark]
    public string Interpolation()
    {
        return $"{_string1}{_string2}{_string3}";
    }

    [Benchmark]
    public string String_Format()
    {
        return string.Format("{0}{1}{2}", _string1, _string2, _string3);
    }
}
