using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<ArrayVsList>();

[SimpleJob, MemoryDiagnoser(false)]
public class ArrayVsList
{
    private const int Size = 1_000_000;
    private int[] _array;
    private List<int> _list;

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random();

        _array = new int[Size];
        _list = new List<int>(Size);

        for (var index = 0; index < Size; index++)
        {
            int value = random.Next();
            _array[index] = value;
            _list.Add(value);
        }
        
        Trace.Assert(_array.Length == Size);
        Trace.Assert(_list.Count == Size);
    }

    [Benchmark]
    public int TotalOfArray()
    {
        var total = 0;
        for (var index = 0; index < Size; index++)
        {
            total += _array[index];
        }
        return total;
    }

    [Benchmark]
    public int TotalOfList()
    {
        var total = 0;
        for (var index = 0; index < Size; index++)
        {
            total += _list[index];
        }
        return total;
    }

    [Benchmark]
    public void SetArrayToZero()
    {
        for (var index = 0; index < Size; index++)
        {
            _array[index] = 0;
        }
    }

    [Benchmark]
    public void SetListToZero()
    {
        for (var index = 0; index < Size; index++)
        {
            _list[index] = 0;
        }
    }
}
