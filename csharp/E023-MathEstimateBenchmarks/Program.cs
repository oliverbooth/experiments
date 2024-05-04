using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<MathEstimateBenchmarks>();

[SimpleJob, MemoryDiagnoser(false)]
public class MathEstimateBenchmarks
{
    [Benchmark, Arguments(10)]
    public double OneOverX(double x) => 1.0 / x;

    [Benchmark, Arguments(10)]
    public double ReciprocalEstimate(double x) => Math.ReciprocalEstimate(x);

    [Benchmark, Arguments(10)]
    public double OneOverSqrtX(double x) => 1.0 / Math.Sqrt(x);

    [Benchmark, Arguments(10)]
    public double ReciprocalSqrtEstimate(double x) => Math.ReciprocalSqrtEstimate(x);
}
