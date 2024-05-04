namespace E031_ArrayVsEnumerable;

public static class ArrayExtensions
{
    public static T[] AsArray<T>(this T value) => new[] { value };
    public static T[] AsArray<T>(this (T, T) value) => new[] { value.Item1, value.Item2 };
    public static T[] AsArray<T>(this (T, T, T) value) => new[] { value.Item1, value.Item2, value.Item3 };
    public static T[] AsArray<T>(this (T, T, T, T) value) => new[] { value.Item1, value.Item2, value.Item3, value.Item4 };

    public static T[] AsArray<T>(this (T, T, T, T, T) value) =>
        new[] { value.Item1, value.Item2, value.Item3, value.Item4, value.Item5 };

    public static T[] AsArray<T>(this (T, T, T, T, T, T) value) => new[]
        { value.Item1, value.Item2, value.Item3, value.Item4, value.Item5, value.Item6 };

    public static T[] AsArray<T>(this (T, T, T, T, T, T, T) value) => new[]
        { value.Item1, value.Item2, value.Item3, value.Item4, value.Item5, value.Item6, value.Item7 };

    public static T[] AsArray<T>(this (T, T, T, T, T, T, T, T) value) => new[]
        { value.Item1, value.Item2, value.Item3, value.Item4, value.Item5, value.Item6, value.Item7, value.Item8 };

    public static T[] AsArray<T>(this (T, T, T, T, T, T, T, T, T) value) => new[]
        { value.Item1, value.Item2, value.Item3, value.Item4, value.Item5, value.Item6, value.Item7, value.Item8, value.Item9 };

    public static T[] AsArray<T>(this (T, T, T, T, T, T, T, T, T, T) value) => new[]
    {
        value.Item1, value.Item2, value.Item3, value.Item4, value.Item5, value.Item6, value.Item7, value.Item8, value.Item9,
        value.Item10
    };

    public static IEnumerable<T> AsEnumerable<T>(this T value)
    {
        yield return value;
    }

    public static IEnumerable<T> AsEnumerable<T>(this (T, T) value)
    {
        yield return value.Item1;
        yield return value.Item2;
    }

    public static IEnumerable<T> AsEnumerable<T>(this (T, T, T) value)
    {
        yield return value.Item1;
        yield return value.Item2;
        yield return value.Item3;
    }

    public static IEnumerable<T> AsEnumerable<T>(this (T, T, T, T) value)
    {
        yield return value.Item1;
        yield return value.Item2;
        yield return value.Item3;
        yield return value.Item4;
    }

    public static IEnumerable<T> AsEnumerable<T>(this (T, T, T, T, T ) value)
    {
        yield return value.Item1;
        yield return value.Item2;
        yield return value.Item3;
        yield return value.Item4;
        yield return value.Item5;
    }

    public static IEnumerable<T> AsEnumerable<T>(this (T, T, T, T, T, T) value)
    {
        yield return value.Item1;
        yield return value.Item2;
        yield return value.Item3;
        yield return value.Item4;
        yield return value.Item5;
        yield return value.Item6;
    }

    public static IEnumerable<T> AsEnumerable<T>(this (T, T, T, T, T, T, T) value)
    {
        yield return value.Item1;
        yield return value.Item2;
        yield return value.Item3;
        yield return value.Item4;
        yield return value.Item5;
        yield return value.Item6;
        yield return value.Item7;
    }

    public static IEnumerable<T> AsEnumerable<T>(this (T, T, T, T, T, T, T, T) value)
    {
        yield return value.Item1;
        yield return value.Item2;
        yield return value.Item3;
        yield return value.Item4;
        yield return value.Item5;
        yield return value.Item6;
        yield return value.Item7;
        yield return value.Item8;
    }

    public static IEnumerable<T> AsEnumerable<T>(this (T, T, T, T, T, T, T, T, T) value)
    {
        yield return value.Item1;
        yield return value.Item2;
        yield return value.Item3;
        yield return value.Item4;
        yield return value.Item5;
        yield return value.Item6;
        yield return value.Item7;
        yield return value.Item8;
        yield return value.Item9;
    }

    public static IEnumerable<T> AsEnumerable<T>(this (T, T, T, T, T, T, T, T, T, T) value)
    {
        yield return value.Item1;
        yield return value.Item2;
        yield return value.Item3;
        yield return value.Item4;
        yield return value.Item5;
        yield return value.Item6;
        yield return value.Item7;
        yield return value.Item8;
        yield return value.Item9;
        yield return value.Item10;
    }
}
