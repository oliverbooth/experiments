using BenchmarkDotNet.Attributes;

namespace E002_DiscordUriParser;

[SimpleJob, MemoryDiagnoser(false)]
public class DiscordUriParserBenchmarks
{
    private const string Message = "This is a test https://discord.com/channels/" +
                                   "779115633837211659/815556722722209803/944679403420524654";

    [Benchmark]
    [Arguments(Message)]
    public (ulong, ulong, ulong) UsingUri(string input) => DiscordUrlParser.UsingUri(input);

    [Benchmark]
    [Arguments(Message)]
    public (ulong, ulong, ulong) UsingRegex(string input) => DiscordUrlParser.UsingRegex(input);
}
