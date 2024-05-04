internal class Options
{
    public bool Whitespace { get; set; } = false;

    public IEnumerable<string> Ignore { get; set; } = Array.Empty<string>();

    public string IgnoreChars { get; set; } = string.Empty;

    public string Path { get; set; } = ".";

    public string Pattern { get; set; } = "^.+$";

    public bool Recurse { get; set; } = false;

    public bool Verbose { get; set; } = false;
}
