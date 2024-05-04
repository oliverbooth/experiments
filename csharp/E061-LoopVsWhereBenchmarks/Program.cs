using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<LoopVsWhereBenchmarks>();

[SimpleJob, MemoryDiagnoser(false)]
public class LoopVsWhereBenchmarks
{
    private const string Input = "This is a test hello world 01234\n";

    [Benchmark]
    [Arguments(Input)]
    public string Loop_WithPatternMatch_Concat(string input)
    {
        var errors = 0;
        for (var i = 0; i < input.Length; i++)
        {
            if (input[i] is > 'm' or < 'a')
            {
                errors++;
            }
        }

        return errors + "/" + input.Length;
    }

    [Benchmark]
    [Arguments(Input)]
    public string Loop_WithPatternMatch_Interpolate(string input)
    {
        var errors = 0;
        for (var i = 0; i < input.Length; i++)
        {
            if (input[i] is > 'm' or < 'a')
            {
                errors++;
            }
        }

        return $"{errors}/{input.Length}";
    }

    [Benchmark]
    [Arguments(Input)]
    public string Loop_WithOrOperator_Concat(string input)
    {
        var errors = 0;
        for (var i = 0; i < input.Length; i++)
        {
            if (input[i] > 'm' || input[i] < 'a')
            {
                errors++;
            }
        }

        return errors + "/" + input.Length;
    }

    [Benchmark]
    [Arguments(Input)]
    public string Loop_WithOrOperator_Interpolate(string input)
    {
        var errors = 0;
        for (var i = 0; i < input.Length; i++)
        {
            if (input[i] > 'm' || input[i] < 'a')
            {
                errors++;
            }
        }

        return $"{errors}/{input.Length}";
    }

    [Benchmark]
    [Arguments(Input)]
    public string Count_WithPatternMatch_Concat(string input)
    {
        return input.Count(c => c is > 'm' or < 'a') + "/" + input.Length;
    }

    [Benchmark]
    [Arguments(Input)]
    public string Count_WithPatternMatch_Interpolate(string input)
    {
        return $"{input.Count(c => c is > 'm' or < 'a')}/{input.Length}";
    }

    [Benchmark]
    [Arguments(Input)]
    public string Count_WithOrOperator_Concat(string input)
    {
        return input.Count(c => c > 'm' || c < 'a') + "/" + input.Length;
    }

    [Benchmark]
    [Arguments(Input)]
    public string Count_WithOrOperator_Interpolate(string input)
    {
        return $"{input.Count(c => c > 'm' || c < 'a')}/{input.Length}";
    }
}
