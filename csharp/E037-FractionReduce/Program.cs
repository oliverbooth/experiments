Console.WriteLine(Reduction(7, 16));
return;

static string Reduction(int numerator, int denominator)
{
    for (var i = 10; i > 1; i--)
    {
        while (numerator % i == 0 && denominator % i == 0)
        {
            numerator /= i;
            denominator /= i;
        }
    }

    return $"{numerator}/{denominator}";
}
