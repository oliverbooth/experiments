using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using E031_ArrayVsEnumerable;

BenchmarkRunner.Run<ArrayVsEnumerable>();

[SimpleJob, MemoryDiagnoser(false)]
public class ArrayVsEnumerable
{
    [Benchmark]
    public int[] ConcatWithArraySingleton()
    {
        return 0.AsArray().Concat(5.AsArray()).ToArray();
    }

    [Benchmark]
    public int[] ConcatWithEnumerableSingleton()
    {
        return 0.AsEnumerable().Concat(5.AsEnumerable()).ToArray();
    }

    [Benchmark]
    public int[] ConcatWithArrayTuple()
    {
        return (1, 2, 3).AsArray().Concat((4, 5, 6).AsArray()).ToArray();
    }

    [Benchmark]
    public int[] ConcatWithEnumerableTuple()
    {
        return (1, 2, 3).AsEnumerable().Concat((4, 5, 6).AsEnumerable()).ToArray();
    }
}
