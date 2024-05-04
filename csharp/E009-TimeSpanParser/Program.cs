using Humanizer;
using Humanizer.Localisation;

Console.WriteLine(Parse("2y").Humanize(10, true, minUnit: TimeUnit.Second, maxUnit: TimeUnit.Year));
Console.WriteLine(Parse("2m").Humanize(10, true, minUnit: TimeUnit.Second, maxUnit: TimeUnit.Year));
Console.WriteLine(Parse("2mo").Humanize(10, true, minUnit: TimeUnit.Second, maxUnit: TimeUnit.Year));
Console.WriteLine(Parse("1y2mo3m").Humanize(10, true, minUnit: TimeUnit.Second, maxUnit: TimeUnit.Year));
Console.WriteLine(Parse("3d").Humanize(10, true, minUnit: TimeUnit.Second, maxUnit: TimeUnit.Year));
Console.WriteLine(Parse("1y1mo1w1d1h1m1s").Humanize(10, true, minUnit: TimeUnit.Second, maxUnit: TimeUnit.Year));
return;

static TimeSpan Parse(string value)
{
    TimeSpan result = TimeSpan.Zero;
    var unitValue = 0;

    for (var index = 0; index < value.Length; index++)
    {
        char current = value[index];
        switch (current)
        {
            case var digitChar when char.IsDigit(digitChar):
                var digit = (int)char.GetNumericValue(digitChar);
                unitValue = unitValue * 10 + digit;
                break;

            case 'y':
                result += TimeSpan.FromDays(unitValue * 365);
                unitValue = 0;
                break;

            case 'm':
                if (index < value.Length - 1 && value[index + 1] == 'o')
                {
                    index++;
                    result += TimeSpan.FromDays(unitValue * 30);
                }
                else
                {
                    result += TimeSpan.FromMinutes(unitValue);
                }

                unitValue = 0;
                break;

            case 'w':
                result += TimeSpan.FromDays(unitValue * 7);
                unitValue = 0;
                break;

            case 'd':
                result += TimeSpan.FromDays(unitValue);
                unitValue = 0;
                break;

            case 'h':
                result += TimeSpan.FromHours(unitValue);
                unitValue = 0;
                break;

            case 's':
                result += TimeSpan.FromSeconds(unitValue);
                unitValue = 0;
                break;
        }
    }

    return result;
}
