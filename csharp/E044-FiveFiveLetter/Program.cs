const string Url = "https://raw.githubusercontent.com/dwyl/english-words/master/words_alpha.txt";
var letters = new Dictionary<char, int>();
using var httpClient = new HttpClient();

var words = new List<string>();

if (File.Exists("words_alpha.txt"))
{
    Console.WriteLine("Loading word list...");
    words = (await File.ReadAllLinesAsync("words_alpha.txt").ConfigureAwait(false)).ToList();
    Console.WriteLine($"Loaded {words.Count} words");
}
else
{
    Console.WriteLine("Downloading word list...");

    await using Stream stream = await httpClient.GetStreamAsync(Url).ConfigureAwait(false);
    using var reader = new StreamReader(stream);
    while (!reader.EndOfStream && await reader.ReadLineAsync().ConfigureAwait(false) is { } line)
        words.Add(line);

    await File.WriteAllLinesAsync("words_alpha.txt", words).ConfigureAwait(false);
    Console.WriteLine($"Downloaded {words.Count} words");
}

Console.WriteLine("Removing words greater than 5 characters...");
words.RemoveAll(word => word.Length > 5);
Console.WriteLine($"Remaining {words.Count} words");

Console.WriteLine("Removing words with duplicate letters...");
words.RemoveAll(word => word.Length != new HashSet<char>(word).Count);
Console.WriteLine($"Remaining {words.Count} words");

Console.WriteLine("Removing anagrams...");
words.RemoveAll(word => words.Any(other => other != word && other.Length == word.Length && IsAnagram(word, other)));
Console.WriteLine($"Remaining {words.Count} words");

Console.WriteLine("Searching for 5-letter words...");

var currentSet = new Queue<string>();
foreach (string word in words)
{
    currentSet.Enqueue(word);
    foreach (string other in words)
    {
        currentSet.Enqueue(other);
        if (word == other)
        {
            currentSet.Dequeue();
            continue;
        }

        if (word.Any(c => other.Contains(c)))
        {
            currentSet.Dequeue();
        }
    }
}

return;

bool IsAnagram(string word, string other)
{
    letters.Clear();
    foreach (char letter in word) letters[letter] = letters.TryGetValue(letter, out int count) ? count + 1 : 1;
    foreach (char letter in other) letters[letter] = letters.TryGetValue(letter, out int count) ? count - 1 : -1;
    bool result = letters.Values.All(count => count == 0);
    return result;
}
