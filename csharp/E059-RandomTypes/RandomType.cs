namespace E059_RandomTypes;

public struct RandomType
{
    private static readonly Type[] Types = { typeof(int), typeof(double), typeof(bool), typeof(string) };
    private static readonly Random Random = new();
    private readonly Type _type;
    private readonly object _value;

    private RandomType(Type type, object value)
    {
        _type = type;
        _value = value;
    }

    public static explicit operator int(RandomType r)
    {
        if (r._type == typeof(int)) return (int)r._value;
        throw new InvalidCastException("Wrong, dipshit");
    }

    public static explicit operator double(RandomType r)
    {
        if (r._type == typeof(double)) return (double)r._value;
        throw new InvalidCastException("Wrong, dipshit");
    }

    public static explicit operator bool(RandomType r)
    {
        if (r._type == typeof(bool)) return (bool)r._value;
        throw new InvalidCastException("Wrong, dipshit");
    }

    public static explicit operator string(RandomType r)
    {
        if (r._type == typeof(string)) return (string)r._value;
        throw new InvalidCastException("Wrong, dipshit");
    }

    public static implicit operator RandomType(int o) => new RandomType(Types[Random.Next(Types.Length)], o);
    public static implicit operator RandomType(bool o) => new RandomType(Types[Random.Next(Types.Length)], o);
    public static implicit operator RandomType(double o) => new RandomType(Types[Random.Next(Types.Length)], o);
    public static implicit operator RandomType(string o) => new RandomType(Types[Random.Next(Types.Length)], o);
}
