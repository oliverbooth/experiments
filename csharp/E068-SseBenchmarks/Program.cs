using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<SseBenchmarks>();

[SimpleJob, MemoryDiagnoser(false)]
public class SseBenchmarks
{
    private readonly int[] _firstArray = { 1, 2, 3, 4 };
    private readonly int[] _secondArray = { 5, 6, 7, 8 };

    [Benchmark]
    public int[] ClassicLoop()
    {
        var result = new int[4];

        for (var index = 0; index < _firstArray.Length; index++)
        {
            result[index] = _firstArray[index] + _secondArray[index];
        }

        return result;
    }

    [Benchmark]
    public int[] Vectorized()
    {
        var result = new int[4];

        unsafe
        {
            fixed (int* ap = _firstArray, bp = _secondArray, rp = result)
            {
                int vectorCount = _firstArray.Length / Vector128<int>.Count;
                for (var i = 0; i < vectorCount; i++)
                {
                    Vector128<int> v1 = Sse2.LoadVector128(ap + i);
                    Vector128<int> v2 = Sse2.LoadVector128(bp + i);
                    Vector128<int> sum = Sse2.Add(v1, v2);
                    Sse2.Store(rp + i, sum);
                }
            }
        }

        return result;
    }
}
