using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<CoordinateBenchmarks>();

[SimpleJob, MemoryDiagnoser(false)]
public class CoordinateBenchmarks
{
    [Benchmark]
    public OldCoordinates OldParse()
    {
        return OldCoordinates.Parse("Mutation 0n 0e 0a 0");
    }

    [Benchmark]
    public Coordinates NewParse()
    {
        return Coordinates.Parse("Mutation 0n 0e 0a 0");
    }
}
