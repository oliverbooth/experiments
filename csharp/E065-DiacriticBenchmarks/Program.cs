using System.Globalization;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<DiacriticBenchmarks>();

[SimpleJob, MemoryDiagnoser(false)]
public class DiacriticBenchmarks
{
    private const string Sample = "ἠἡὀὁἱἰὠὡἐἑὑὐᾐ";

    [Benchmark]
    [Arguments(Sample)]
    public string StackOverflow_RemoveDiacritics(string input)
    {
        string normalizedString = input.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

        for (var i = 0; i < normalizedString.Length; i++)
        {
            char c = normalizedString[i];
            UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    [Benchmark]
    [Arguments(Sample)]
    public string Boas_RemoveSpiritus(string input)
    {
        string output = MyReplace(input, "ἀἁ", "α");
        output = MyReplace(output, "ἠἡ", "η");
        output = MyReplace(output, "ὀὁ", "ο");
        output = MyReplace(output, "ἱἰ", "ι");
        output = MyReplace(output, "ὠὡ", "ω");
        output = MyReplace(output, "ἐἑ", "ε");
        output = MyReplace(output, "ὑὐ", "υ");
        output = MyReplace(output, "ᾐ", "ῃ");
        return output;
    }

    private string MyReplace(string input, string pattern, string replacement)
    {
        var sb = new StringBuilder();
        foreach (char t in input)
        {
            if (!pattern.Contains(t))
            {
                sb.Append(t);
            }
            else
            {
                sb.Append(replacement);
            }
        }

        return sb.ToString();
    }
}
