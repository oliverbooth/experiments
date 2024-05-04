using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.Configuration;

BenchmarkRunner.Run<ConfigurationBenchmarks>();

[SimpleJob, MemoryDiagnoser(false)]
public class ConfigurationBenchmarks
{
    private IConfiguration _configuration = null!;

    [GlobalSetup]
    public void Setup()
    {
        const string rawJson = """
                               {
                                   "foo": {
                                       "bar": {
                                           "value": "Hello World"
                                       }
                                   }
                               }
                               """;

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(rawJson));
        _configuration = new ConfigurationBuilder().AddJsonStream(stream).Build();
    }

    [Benchmark]
    public string? NestedCalls()
    {
        return _configuration.GetSection("foo").GetSection("bar").GetSection("value").Value;
    }

    [Benchmark]
    public string? SingleCall()
    {
        return _configuration.GetSection("foo:bar:value").Value;
    }
}
