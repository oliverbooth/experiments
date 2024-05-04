using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;

namespace E015_RegexVsCustomAttributeParser;

[SimpleJob, MemoryDiagnoser(false)]
public class RegexVsCustomAttributeParser
{
    private Regex _regex;

    [GlobalSetup]
    public void Setup()
    {
        _regex = new Regex(@"^\[[A-Z_][A-Z0-9_]+(\(([0-9]+)\))?\]$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }

    [Benchmark]
    [Arguments("[UseResources]")]
    [Arguments("[UseResources(42)]")]
    [Arguments("[UseResources(420)]")]
    [Arguments("[UseResources(4200)]")]
    public int Regex(string input)
    {
        Match match = _regex.Match(input);
        if (!match.Success) return 0;
        if (match.Groups.Count < 3) return 0;

        Group argumentsGroup = match.Groups[1];
        Group firstArgumentGroup = match.Groups[2];

        if (!argumentsGroup.Success || !firstArgumentGroup.Success) return 0;
        return int.TryParse(firstArgumentGroup.ValueSpan, out int result) ? result : 0;
    }

    /*[Benchmark]
[Arguments("[UseResources]")]
[Arguments("[UseResources(42)]")]
[Arguments("[UseResources(420)]")]
[Arguments("[UseResources(4200)]")]
public int CustomParser_GetNumericValue(string input)
{
    if (input[0] != '[' || input[^1] != ']') return 0;

    var argumentList = false;
    var result = 0;

    for (var index = 1; index < input.Length - 1; index++)
    {
        char current = input[index];
        if (current == '(')
        {
            if (argumentList) return 0;
            argumentList = true;
            continue;
        }

        if (current == ')')
        {
            if (!argumentList) return 0;
            argumentList = false;
            continue;
        }

        if (argumentList)
        {
            if (!char.IsDigit(current)) return 0;

            var numericValue = (int) char.GetNumericValue(current);
            result = result * 10 + numericValue;
        }
    }

    return result;
}*/

    [Benchmark]
    [Arguments("[UseResources]")]
    [Arguments("[UseResources(42)]")]
    [Arguments("[UseResources(420)]")]
    [Arguments("[UseResources(4200)]")]
    public int CustomParser(string input)
    {
        if (input[0] != '[' || input[^1] != ']') return 0;

        var argumentList = false;
        var result = 0;

        for (var index = 1; index < input.Length - 1; index++)
        {
            char current = input[index];
            if (current == '(')
            {
                if (argumentList) return 0;
                argumentList = true;
                continue;
            }

            if (current == ')')
            {
                if (!argumentList) return 0;
                argumentList = false;
                continue;
            }

            if (argumentList)
            {
                if (current is < '0' or > '9') return 0;

                int numericValue = current - '0';
                result = result * 10 + numericValue;
            }
        }

        return result;
    }
}
