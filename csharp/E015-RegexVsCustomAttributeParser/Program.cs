using System.Text.RegularExpressions;
using BenchmarkDotNet.Running;
using E015_RegexVsCustomAttributeParser;

Regex regex = new(@"^\[[A-Z_][A-Z0-9_]+(\(([0-9]+)\))?\]$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

Console.WriteLine(UsingRegex("[UseResources]"));
Console.WriteLine(UsingRegex("[UseResources(42)]"));
Console.WriteLine(UsingRegex("[UseResources(420)]"));
Console.WriteLine(UsingRegex("[UseResources(4200)]"));

Console.WriteLine(UsingCustomParser("[UseResources]"));
Console.WriteLine(UsingCustomParser("[UseResources(42)]"));
Console.WriteLine(UsingCustomParser("[UseResources(420)]"));
Console.WriteLine(UsingCustomParser("[UseResources(4200)]"));

BenchmarkRunner.Run<RegexVsCustomAttributeParser>();
return;

int UsingRegex(string input)
{
    Match match = regex.Match(input);
    if (!match.Success) return 0;
    if (match.Groups.Count < 3) return 0;

    Group argumentsGroup = match.Groups[1];
    Group firstArgumentGroup = match.Groups[2];

    if (!argumentsGroup.Success || !firstArgumentGroup.Success) return 0;
    return int.TryParse(firstArgumentGroup.ValueSpan, out int result) ? result : 0;
}

static int UsingCustomParser(string input)
{
    ReadOnlySpan<char> span = input.AsSpan();
    if (span[0] != '[' || span[^1] != ']') return 0;

    var argumentList = false;
    var result = 0;


    for (var index = 1; index < span.Length - 1; index++)
    {
        char current = span[index];
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
