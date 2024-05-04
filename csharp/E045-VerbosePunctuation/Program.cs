using System.Text;

while (true)
{
    Console.ResetColor();
    string input = Console.ReadLine()!;

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(Verbosify(input));
}

static string Verbosify(string input)
{
    while (input.Contains("..."))
        input = input.Replace("...", "…");

    input = input.Replace("?!", "‽");
    input = input.Replace("!?", "‽");
    input = input.Replace("?‽", "‽");
    input = input.Replace("!‽", "‽");
    input = input.Replace('“', '"'); 
    input = input.Replace('”', '"');
    input = input.Replace('‘', '\'');
    input = input.Replace('’', '\'');

    var builder = new StringBuilder();
    foreach (char character in input)
    {
        if (char.IsLetter(character)) builder.Append(character);
        else
        {
            switch (character)
            {
                case '0': builder.Append(" zero "); break;
                case '1': builder.Append(" one "); break;
                case '2': builder.Append(" two "); break;
                case '3': builder.Append(" three "); break;
                case '4': builder.Append(" four "); break;
                case '5': builder.Append(" five "); break;
                case '6': builder.Append(" six "); break;
                case '7': builder.Append(" seven "); break;
                case '8': builder.Append(" eight "); break;
                case '9': builder.Append(" nine "); break;
                case '…': builder.Append(" ellipsis "); break;
                case '.': builder.Append(" period "); break;
                case ',': builder.Append(" comma "); break;
                case ':': builder.Append(" colon "); break;
                case ';': builder.Append(" semicolon "); break;
                case '‽': builder.Append(" interrobang "); break;
                case '!': builder.Append(" exclamation mark "); break;
                case '?': builder.Append(" question mark "); break;
                case '\'': builder.Append(" apostrophe "); break;
                case '"': builder.Append(" quotation mark "); break;
                case '-': builder.Append(" hyphen "); break;
                case '_': builder.Append(" underscore "); break;
                case '(': builder.Append(" open parenthesis "); break;
                case ')': builder.Append(" close parenthesis "); break;
                case '{': builder.Append(" open brace "); break;
                case '}': builder.Append(" close brace "); break;
                case '[': builder.Append(" open bracket "); break;
                case ']': builder.Append(" close bracket "); break;
                case '<': builder.Append(" open chevon "); break;
                case '>': builder.Append(" close chevon "); break;
                case '+': builder.Append(" plus "); break;
                case '=': builder.Append(" equals "); break;
                case '*': builder.Append(" asterisk "); break;
                case '%': builder.Append(" percent "); break;
                case '$': builder.Append(" dollar "); break;
                case '#': builder.Append(" hash "); break;
                case '@': builder.Append(" at  "); break;
                case '&': builder.Append(" ampersand "); break;
                case '|': builder.Append(" pipe  "); break;
                case '\\': builder.Append(" backslash "); break;
                case '/': builder.Append(" slash "); break;
                case '^': builder.Append(" caret "); break;
                case '~': builder.Append(" tilde "); break;
                case '`': builder.Append(" grave accent "); break;
                default: builder.Append(character); break;
            }
        }
    }

    var result = builder.ToString();
    while (result.Contains("  "))
        result = result.Replace("  ", " ");

    return result.Trim();
}