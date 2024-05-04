using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<LinqVsNoLinqBenchmarks>();

[SimpleJob, MemoryDiagnoser(false)]
public class LinqVsNoLinqBenchmarks
{
    private readonly List<User> _users = new();

    [GlobalSetup]
    public void Setup()
    {
        _users.Clear();
        for (var index = 0; index < 500_000; index++)
        {
            _users.Add(new User { Id = index.ToString() });
        }
    }

    [Benchmark]
    public int Linq()
    {
        return _users.Where(u => u.Id == "10").Select(u => int.Parse(u.Id)).Sum();
    }

    [Benchmark]
    public int Loop()
    {
        var total = 0;

        for (var index = 0; index < _users.Count; index++)
        {
            string id = _users[index].Id;

            if (id == "10")
                total += int.Parse(id);
        }

        return total;
    }

    private class User
    {
        public string Id { get; init; }
    }
}
