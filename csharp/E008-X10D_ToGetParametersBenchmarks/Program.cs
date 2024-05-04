using System.Web;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<X10DToGetParametersBenchmarks>();

[SimpleJob, MemoryDiagnoser(false)]
public class X10DToGetParametersBenchmarks
{
    public Dictionary<string, int> Dictionary { get; } = new();

    [GlobalSetup]
    public void Setup()
    {
        for (var index = 0; index < 26; index++)
            Dictionary.Add(('a' + index).ToString(), index);
    }

    [Benchmark]
    public string List()
    {
        static string Sanitize<TKey, TValue>(KeyValuePair<TKey, TValue> pair) where TKey : notnull
        {
            string key = HttpUtility.UrlEncode(pair.Key.ToString())!;
            string? value = HttpUtility.UrlEncode(pair.Value?.ToString());
            return $"{key}={value}";
        }

        var list = new List<string>();
        foreach (var pair in Dictionary)
        {
            list.Add(Sanitize(pair));
        }

        return string.Join('&', list);
    }

    [Benchmark]
    public string Linq()
    {
        static string Sanitize<TKey, TValue>(KeyValuePair<TKey, TValue> pair) where TKey : notnull
        {
            string key = HttpUtility.UrlEncode(pair.Key.ToString())!;
            string? value = HttpUtility.UrlEncode(pair.Value?.ToString());
            return $"{key}={value}";
        }

        return string.Join('&', Dictionary.Select(Sanitize));
    }
}
