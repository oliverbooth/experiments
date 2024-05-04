using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<LineCountBenchmarks>();

[SimpleJob, MemoryDiagnoser(false)]
public class LineCountBenchmarks
{
    private readonly Options _options = new()
    {
        Path = Environment.GetEnvironmentVariable("TEST_PATH") ?? ".",
        Pattern = "\\.cs$"
    };

    [Benchmark]
    public async Task<int> CountLinesAsync()
    {
        var regex = new Regex(_options.Pattern, RegexOptions.Compiled);
        var path = Path.GetFullPath(_options.Path);
        var searchOption = _options.Recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        var files = Directory.GetFiles(path, "*", searchOption);
        var count = 0;

        foreach (var file in files)
        {
            var directory = Path.GetDirectoryName(file);
            if (directory is null)
            {
                continue;
            }

            if (_options.Ignore.Select(Path.GetFullPath).Any(i => directory.StartsWith(i)))
            {
                continue;
            }

            if (!regex.IsMatch(file))
            {
                continue;
            }

            var lines = (await File.ReadAllLinesAsync(file).ConfigureAwait(false)) as IEnumerable<string>;

            if (!_options.Whitespace)
            {
                lines = lines.Where(line => !string.IsNullOrWhiteSpace(line));
            }

            if (!string.IsNullOrWhiteSpace(_options.IgnoreChars))
            {
                lines = lines.Where(line => line.Trim().Length > 0 && _options.IgnoreChars.IndexOf(line.Trim()[0]) != 0);
            }

            var fileCount = lines.Count();
            count += fileCount;
        }

        return count;
    }

    [Benchmark]
    public int Count()
    {
        var regex = new Regex(_options.Pattern, RegexOptions.Compiled);
        string path = Path.GetFullPath(_options.Path);
        var searchOption = _options.Recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        string[] files = Directory.GetFiles(path, "*", searchOption);
        var count = 0;
        var ignoreChars = _options.IgnoreChars.AsSpan();

        foreach (string file in files)
        {
            count = CountLinesInFile(file, _options, regex, ignoreChars, count);
        }

        return count;
    }

    private static int CountLinesInFile(string file, Options options, Regex regex, ReadOnlySpan<char> ignoreChars, int count)
    {
        string? directory = Path.GetDirectoryName(file);

        if (directory is null)
        {
            return count;
        }

        string directoryFullPath = Path.GetFullPath(directory);
        var ignore = false;
        foreach (string i in options.Ignore)
        {
            if (directoryFullPath.StartsWith(Path.GetFullPath(i)))
            {
                ignore = true;
                break;
            }
        }

        if (ignore)
        {
            return count;
        }

        if (!regex.IsMatch(file))
        {
            return count;
        }

        var fileCount = 0;
        using (var reader = new StreamReader(file))
        {
            while (reader.ReadLine() is { } line)
            {
                var lineSpan = line.AsSpan().Trim();
                if (lineSpan.Length == 0)
                {
                    continue;
                }

                if (options.IgnoreChars.Length > 0 && ignoreChars.IndexOf(lineSpan[0]) != -1)
                {
                    continue;
                }

                fileCount++;
            }
        }

        count += fileCount;
        return count;
    }
}
