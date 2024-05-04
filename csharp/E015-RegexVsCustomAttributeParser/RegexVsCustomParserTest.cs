using System.Text.RegularExpressions;

public class RegexVsCustomParserTest
{
    private static readonly Regex Regex = new(@"^\[[A-Z_][A-Z0-9_]+(\(([0-9]+)\))?\]$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private static void Main()
    {
    }

}
