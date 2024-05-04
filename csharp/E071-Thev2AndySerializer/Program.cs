using System.Text;
using Cysharp.Text;

// placeholder config map
var dictionary = new Dictionary<string, string>
{
    { "Foo", "${Dollar}{Newline}" }
};

using var stream = new MemoryStream();

// serialize
Serialize(stream, dictionary);
Console.WriteLine(Encoding.UTF8.GetString(stream.ToArray()));

// deserialize back
stream.Position = 0; // reset stream position
Dictionary<string, string> deserialized = Deserialize(stream);

// print deserialized to see if it worked
foreach ((string key, string value) in deserialized)
{
    Console.WriteLine($"Key: {key}      Value: {value}");
}

return;

static Dictionary<string, string> Deserialize(Stream stream)
{
    var dictionary = new Dictionary<string, string>();
    using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);

    while (!reader.EndOfStream)
    {
        ReadOnlySpan<char> line = reader.ReadLine().AsSpan();

        int equalsIndex = line.IndexOf('=');
        if (equalsIndex == -1)
        {
            throw new FormatException("Invalid format");
        }

        ReadOnlySpan<char> key = line[..equalsIndex];
        ReadOnlySpan<char> value = line[(equalsIndex + 1)..];

        dictionary.Add(ReadToken(key), ReadToken(value));
    }

    return dictionary;
}

static string ReadToken(ReadOnlySpan<char> token)
{
    using Utf8ValueStringBuilder buffer = ZString.CreateUtf8StringBuilder();
    using Utf8ValueStringBuilder tokenBuffer = ZString.CreateUtf8StringBuilder();
    var insideEscape = false;

    for (var index = 0; index < token.Length; index++)
    {
        char current = token[index];
        switch (current)
        {
            case '$' when !insideEscape && index + 1 < token.Length && token[index + 1] == '{':
                insideEscape = true;
                index++; // skip next {
                break;

            case '}' when insideEscape:
                insideEscape = false; // end of sequence
                buffer.Append(CreateToken(tokenBuffer.AsSpan()));
                tokenBuffer.Clear();
                break;

            case var _ when insideEscape:
                tokenBuffer.Append(current);
                break;

            default:
                buffer.Append(current);
                break;
        }
    }

    if (insideEscape)
    {
        throw new FormatException("Invalid escape sequence");
    }

    return buffer.ToString();
}

static string CreateToken(ReadOnlySpan<byte> escaped)
{
    Span<char> chars = stackalloc char[escaped.Length];
    Encoding.UTF8.GetChars(escaped, chars);

    return chars switch
    {
        "Newline" => "\n",
        "Dollar" => "$",
        "Equals" => "=",
        _ => throw new FormatException("Invalid escape sequence")
    };
}

static void Serialize(Stream destination, Dictionary<string, string> config)
{
    using var writer = new StreamWriter(destination, Encoding.UTF8, leaveOpen: true);

    foreach ((string key, string value) in config)
    {
        WriteToken(writer, key);
        writer.Write('=');
        WriteToken(writer, value);
        writer.WriteLine();
    }
}

static void WriteToken(TextWriter writer, ReadOnlySpan<char> token)
{
    for (var index = 0; index < token.Length; index++)
    {
        char current = token[index];
        switch (current)
        {
            case '=':
                writer.Write("${Equals}");
                break;

            case '$':
                writer.Write("${Dollar}");
                break;

            case '\r':
                // discard, we can handle this with \n branch
                break;

            case '\n':
                writer.Write("${Newline}");
                break;

            default:
                writer.Write(current);
                break;
        }
    }
}
