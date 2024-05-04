using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<AllNumericExceptBenchmarks>();

[SimpleJob, MemoryDiagnoser(false)]
public class AllNumericExceptBenchmarks
{
    [Benchmark]
    [Arguments("1234567890a", 10)]
    [Arguments("12345678901", 10)]
    [Arguments("a2345678901", 10)]
    [Arguments("12345678901", 10)]
    [Arguments("aaaaaaaaaaa", 10)]
    public bool AlphaAnar(string str, int charIndex)
    {
        for (var index = 0; index < str.Length; index++)
        {
            if (char.IsDigit(str[index]) ^ index != charIndex)
                return false;
        }

        return true;
    }

    [Benchmark]
    [Arguments("1234567890a", 10)]
    [Arguments("12345678901", 10)]
    [Arguments("a2345678901", 10)]
    [Arguments("12345678901", 10)]
    [Arguments("aaaaaaaaaaa", 10)]
    public bool Yasahiro(string str, int charIndex)
    {
        for (var index = 0; index < str.Length; index++)
        {
            if (!char.IsDigit(str[index]) ^ index == charIndex)
                return false;
        }

        return true;
    }
}
