using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<InverseSqrtBenchmarks>();

[SimpleJob, MemoryDiagnoser(false)]
public class InverseSqrtBenchmarks
{
    [Benchmark]
    [Arguments(16.0f)]
    public float OneOverMathFSqrt(float number)
    {
        return 1.0f / MathF.Sqrt(number);
    }

    [Benchmark]
    [Arguments(16.0f)]
    public float Q_rsqrt(float number)
    {
        int i;
        float x2, y;
        const float threehalfs = 1.5F;

        x2 = number * 0.5F;
        y = number;
        i = Unsafe.As<float, int>(ref y); // evil floating point bit level hacking
        i = 0x5f3759df - (i >> 1); // what the fuck?
        y = Unsafe.As<int, float>(ref i);
        y = y * (threehalfs - (x2 * y * y)); // 1st iteration
//    y  = y * ( threehalfs - ( x2 * y * y ) );   // 2nd iteration, this can be removed

        return y;
    }
}
