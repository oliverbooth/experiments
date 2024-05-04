namespace E029_CircularBitShift;

internal struct CircularShiftingInt
{
    private int _value;

    public static implicit operator CircularShiftingInt(int value) => new() {_value = value};
    public static implicit operator int(CircularShiftingInt value) => value._value;
    public static int operator <<(CircularShiftingInt value, int shift) => CircularLeftShift(value, shift);
    public static int operator >> (CircularShiftingInt value, int shift) => CircularRightShift(value, shift);

    private static int CircularLeftShift(int value, int shift)
    {
        shift = Mod(shift, 32);
        if (shift == 0) return value;

        var p = 0;
        for (var i = 0; i < shift; i++)
        {
            p |= 1 << (31 - i);
        }

        int cache = value & p;
        cache >>= 32 - shift;
        return (value << shift) | cache;
    }

    private static int CircularRightShift(int value, int shift)
    {
        shift = Mod(shift, 32);
        if (shift == 0) return value;

        var p = 0;
        for (var i = 0; i < shift; i++)
        {
            p |= 1 << i;
        }

        int cache = value & p;
        cache <<= 32 - shift;
        return (value >> shift) | cache;
    }

    private static int Mod(int x, int m)
    {
        int r = x % m;
        return r < 0 ? r + m : r;
    }
}