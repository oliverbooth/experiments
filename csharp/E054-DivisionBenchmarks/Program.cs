using System.Numerics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<DivisionBenchmarks>();

[SimpleJob, MemoryDiagnoser(false)]
public class DivisionBenchmarks
{
    [Benchmark]
    [Arguments(1.0f)]
    public float SingleDivide(float x)
    {
        return x / 10.0f;
    }

    [Benchmark]
    [Arguments(0.1f)]
    public float SingleMultiply(float x)
    {
        return x * 10.0f;
    }

    [Benchmark]
    [Arguments(10.0f)]
    public Vector3 VectorDivide(float y)
    {
        return new Vector3(10.0f, 10.0f, 10.0f) / y;
    }

    [Benchmark]
    [Arguments(0.1f)]
    public Vector3 VectorMultiply(float y)
    {
        return new Vector3(10.0f, 10.0f, 10.0f) * y;
    }
}
