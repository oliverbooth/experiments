using System.Text.RegularExpressions;

namespace E002_DiscordUriParser;

public partial class DiscordUrlParser
{
    private static readonly Regex Regex = GetUrlRegex();

    public static (ulong, ulong, ulong) UsingRegex(string input)
    {
        Match match = Regex.Match(input);
        if (!match.Success)
        {
            return (0, 0, 0);
        }

        return (ulong.Parse(match.Groups[1].Value), ulong.Parse(match.Groups[2].Value), ulong.Parse(match.Groups[3].Value));
    }

    public static (ulong, ulong, ulong) UsingUri(string input)
    {
        string[] words = input.Split(' ');
        foreach (string word in words)
        {
            if (!Uri.IsWellFormedUriString(word, UriKind.Absolute))
            {
                continue;
            }

            var uri = new Uri(word);
            string host = uri.Host;
            if (host.IndexOf('.') != host.LastIndexOf('.'))
            {
                // fuck your subdomains
                host = host[(host.LastIndexOf('.', host.LastIndexOf('.', host.Length - 1) - 1) + 1)..];
            }

            if (host != "discord.com")
            {
                continue;
            }

            string path = uri.AbsolutePath;
            if (!path.StartsWith("/channels/"))
            {
                continue;
            }

            path = path["/channels/".Length..];

            int firstSeparatorIndex = path.IndexOf('/');
            if (firstSeparatorIndex == -1)
            {
                continue;
            }

            int secondSeparatorIndex = path.IndexOf('/', firstSeparatorIndex + 1);
            if (secondSeparatorIndex == -1)
            {
                continue;
            }

            if (ulong.TryParse(path[..firstSeparatorIndex], out ulong guild)
                && ulong.TryParse(path[(firstSeparatorIndex + 1)..secondSeparatorIndex], out ulong channel)
                && ulong.TryParse(path[(secondSeparatorIndex + 1)..], out ulong message))
            {
                return (guild, channel, message);
            }
        }

        return (0, 0, 0);
    }

    /*lang=regex*/
    private const string UrlRegexPattern = @"https://(?:www\.|canary\.|beta\.)?discord.com/channels/([0-9]+)/([0-9]+)/([0-9]+)/?";

    [GeneratedRegex(UrlRegexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-GB")]
    private static partial Regex GetUrlRegex();
}