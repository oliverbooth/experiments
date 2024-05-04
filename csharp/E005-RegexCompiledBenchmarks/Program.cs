using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<RegexCompiledBenchmarks>();

[SimpleJob, MemoryDiagnoser(false)]
public class RegexCompiledBenchmarks
{
    private static readonly Regex RegexInstance = new(@"\d");
    private static readonly Regex CompiledRegexInstance = new(@"\d", RegexOptions.Compiled);

    [Benchmark]
    [Arguments("1234567890")]
    public int BasicRegex(string input)
    {
        return RegexInstance.Matches(input).Count;
    }

    [Benchmark]
    [Arguments("1234567890")]
    public int CompiledRegex(string input)
    {
        return CompiledRegexInstance.Matches(input).Count;
    }
}
