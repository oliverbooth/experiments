using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<DigitalRootBenchmarks>();

[SimpleJob, MemoryDiagnoser(false)]
public class DigitalRootBenchmarks
{
    private const int Number = 2958171;

    [Benchmark]
    [Arguments(Number)]
    public int DigitalRoot_Chars(int number)
    {
        while (number > 9)
        {
            var digits = number.ToString();
            var sum = 0;

            foreach (char digit in digits)
                sum += int.Parse(digit.ToString());

            number = sum;
        }

        return number;
    }

    [Benchmark]
    [Arguments(Number)]
    public int DigitalRoot_Standard(int number)
    {
        while (number > 9)
        {
            var sum = 0;

            for (; number > 0; number /= 10)
                sum += number % 10;

            number = sum;
        }

        return number;
    }

    [Benchmark]
    [Arguments(Number)]
    public int DigitalRoot_Recursion(int number)
    {
        var sum = 0;

        while (number > 0)
        {
            sum += number % 10;
            number /= 10;
        }

        if (sum > 10) sum = DigitalRoot_Recursion(sum);
        return sum;
    }

    [Benchmark]
    [Arguments(Number)]
    public int DigitalRoot_Optimal(int number)
    {
        int result = number % 9;
        return result == 0 ? 9 : result;
    }
}
