using System.Globalization;
using System.Text;
using Cysharp.Text;
using X10D.Linq;
using X10D.Text;

/// <summary>
///     Represents a set of coordinates.
/// </summary>
public readonly struct Coordinates
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Coordinates" /> struct.
    /// </summary>
    /// <param name="x">The X coordinate.</param>
    /// <param name="y">The Y coordinate.</param>
    /// <param name="z">The Z coordinate.</param>
    /// <param name="yaw">The yaw.</param>
    /// <param name="isRelative">
    ///     <see langword="true" /> if these coordinates represent relative coordinates; <see langword="false" /> otherwise.
    /// </param>
    public Coordinates(double x, double y, double z, double yaw, bool isRelative = false)
        : this(null, x, y, z, yaw, isRelative)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Coordinates" /> struct.
    /// </summary>
    /// <param name="world">The world name.</param>
    /// <param name="x">The X coordinate.</param>
    /// <param name="y">The Y coordinate.</param>
    /// <param name="z">The Z coordinate.</param>
    /// <param name="yaw">The yaw.</param>
    /// <param name="isRelative">
    ///     <see langword="true" /> if these coordinates represent relative coordinates; <see langword="false" /> otherwise.
    /// </param>
    public Coordinates(string? world, double x, double y, double z, double yaw, bool isRelative = false)
    {
        World = world;
        X = x;
        Y = y;
        Z = z;
        Yaw = yaw;
        IsRelative = isRelative;
    }

    /// <summary>
    ///     Gets or initializes a value indicating whether this instance represents relative coordinates.
    /// </summary>
    /// <value>
    ///     <see langword="true" /> if this instance represents relative coordinates; otherwise, <see langword="false" />.
    /// </value>
    public bool IsRelative { get; init; }

    /// <summary>
    ///     Gets or initializes the world.
    /// </summary>
    /// <value>The world.</value>
    public string? World { get; init; }

    /// <summary>
    ///     Gets or initializes the X coordinate.
    /// </summary>
    /// <value>The X coordinate.</value>
    public double X { get; init; }

    /// <summary>
    ///     Gets or initializes the Y coordinate.
    /// </summary>
    /// <value>The Y coordinate.</value>
    public double Y { get; init; }

    /// <summary>
    ///     Gets or initializes the yaw.
    /// </summary>
    /// <value>The yaw.</value>
    public double Yaw { get; init; }

    /// <summary>
    ///     Gets or initializes the Z coordinate.
    /// </summary>
    /// <value>The Z coordinate.</value>
    public double Z { get; init; }

    public static bool operator ==(Coordinates left, Coordinates right) =>
        left.Equals(right);

    public static bool operator !=(Coordinates left, Coordinates right) =>
        !(left == right);

    /// <summary>
    ///     Parses a coordinate string.
    /// </summary>
    /// <param name="coordinates">The coordinates to parse.</param>
    /// <returns>An instance of <see cref="Coordinates" />.</returns>
    public static Coordinates Parse(string coordinates)
    {
        return Serializer.Deserialize(coordinates);
    }

    /// <summary>
    ///     Returns a value indicating whether this instance of <see cref="Coordinates" /> and another instance of
    ///     <see cref="Coordinates" /> are equal.
    /// </summary>
    /// <param name="other">The instance against which to compare.</param>
    /// <returns>
    ///     <see langword="true" /> if this instance is equal to <paramref name="other" />; otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals(Coordinates other)
    {
        return X.Equals(other.X) &&
               Y.Equals(other.Y) &&
               Z.Equals(other.Z) &&
               Yaw.Equals(other.Yaw) &&
               IsRelative.Equals(other.IsRelative) &&
               string.Equals(World, other.World);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is Coordinates other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(World, X, Y, Z, Yaw);
    }

    /// <summary>
    ///     Returns the string representation of these coordinates.
    /// </summary>
    /// <returns>A <see cref="string" /> representation of these coordinates.</returns>
    public override string ToString()
    {
        return ToString("{0}");
    }

    /// <summary>
    ///     Returns the string representation of these coordinates.
    /// </summary>
    /// <param name="format">The format to apply to each component.</param>
    /// <returns>A <see cref="string" /> representation of these coordinates.</returns>
    public string ToString(string format)
    {
        return Serializer.Serialize(this, format);
    }

    internal static class Serializer
    {
        public static string Serialize(in Coordinates coordinates, string format)
        {
            int count = Serialize(coordinates, format, Span<char>.Empty);
            Span<char> chars = stackalloc char[count];
            Serialize(coordinates, format, chars);
            return chars.ToString();
        }

        public static int Serialize(in Coordinates coordinates, string format, Span<char> destination)
        {
            using Utf8ValueStringBuilder builder = ZString.CreateUtf8StringBuilder();

            if (!string.IsNullOrWhiteSpace(coordinates.World))
            {
                builder.Append(coordinates.World);
                builder.Append(' ');
            }

            bool north = coordinates.Z >= 0.0;
            bool west = coordinates.X >= 0.0;
            bool up = coordinates.Y >= 0.0;
            bool dir = coordinates.Yaw >= 0.0;

            if (coordinates.IsRelative)
            {
                if (north)
                {
                    builder.Append('+');
                }

                builder.Append(string.Format(CultureInfo.InvariantCulture, format, coordinates.Z));
                builder.Append(' ');

                if (west)
                {
                    builder.Append('+');
                }

                builder.Append(string.Format(CultureInfo.InvariantCulture, format, coordinates.X));
                builder.Append(' ');

                if (up)
                {
                    builder.Append('+');
                }

                builder.Append(string.Format(CultureInfo.InvariantCulture, format, coordinates.Y));
                builder.Append("a ");

                if (up)
                {
                    builder.Append('+');
                }

                builder.Append(string.Format(CultureInfo.InvariantCulture, format, coordinates.Yaw));
            }
            else
            {
                char zChar = north ? 'n' : 's';
                char xChar = west ? 'w' : 'e';

                builder.Append(string.Format(CultureInfo.InvariantCulture, format, Math.Abs(coordinates.Z)));
                builder.Append(zChar);
                builder.Append(' ');

                builder.Append(string.Format(CultureInfo.InvariantCulture, format, Math.Abs(coordinates.X)));
                builder.Append(xChar);
                builder.Append(' ');

                builder.Append(string.Format(CultureInfo.InvariantCulture, format, coordinates.Y));
                builder.Append("a ");

                builder.Append(string.Format(CultureInfo.InvariantCulture, format, coordinates.Yaw));
            }

            ReadOnlySpan<byte> bytes = builder.AsSpan();
            Span<char> chars = stackalloc char[bytes.Length];
            Encoding.UTF8.GetChars(bytes, chars);

            for (var index = 0; index < destination.Length; index++)
            {
                destination[index] = chars[index];
            }

            return builder.Length;
        }

        public static Coordinates Deserialize(ReadOnlySpan<char> value)
        {
            using Utf8ValueStringBuilder builder = ZString.CreateUtf8StringBuilder();
            string? world = null;
            var isRelative = false;
            double x = 0.0, y = 0.0, z = 0.0, yaw = 0.0;

            var word = 0;
            for (var index = 0; index < value.Length; index++)
            {
                char current = value[index];
                bool atEnd = index == value.Length - 1;

                if (atEnd || char.IsWhiteSpace(current))
                {
                    if (!builder.AsSpan().All(b => char.IsWhiteSpace((char)b)))
                    {
                        if (atEnd)
                        {
                            builder.Append(current);
                        }

                        ProcessBuffer();
                        word++;
                    }

                    builder.Clear();
                }
                else
                {
                    builder.Append(current);
                }
            }

            return new Coordinates(world, x, y, z, yaw, isRelative);

            void ProcessBuffer()
            {
                ReadOnlySpan<byte> bytes = builder.AsSpan();
                Span<char> chars = stackalloc char[bytes.Length];
                Encoding.UTF8.GetChars(bytes, chars);
                bool hasWorld = !string.IsNullOrWhiteSpace(world);

                if (word == 0 && !IsUnitString(bytes))
                {
                    world = chars.ToString().AsNullIfWhiteSpace();
                }
                else if (IsRelativeUnit(bytes))
                {
                    isRelative = true;

                    switch (word)
                    {
                        case 0 when !hasWorld:
                        case 1 when hasWorld:
                            double.TryParse(chars, NumberStyles.Float, CultureInfo.InvariantCulture, out z);
                            break;
                        case 1 when !hasWorld:
                        case 2 when hasWorld:
                            double.TryParse(chars, NumberStyles.Float, CultureInfo.InvariantCulture, out x);
                            break;
                        case 2 when !hasWorld:
                        case 3 when hasWorld:
                            double.TryParse(chars, NumberStyles.Float, CultureInfo.InvariantCulture, out y);
                            break;
                        case 3 when !hasWorld:
                        case 4 when hasWorld:
                            double.TryParse(chars, NumberStyles.Float, CultureInfo.InvariantCulture, out yaw);
                            break;
                    }
                }
                else
                {
                    if (((!hasWorld && word == 1) || (hasWorld && word == 2)) && chars[^1] is 'x' or 'X' or 'w' or 'W')
                    {
                        _ = double.TryParse(chars[..^1], NumberStyles.Float, CultureInfo.InvariantCulture, out x);
                    }
                    else if (((!hasWorld && word == 0) || (hasWorld && word == 1)) && chars[^1] is 'z' or 'Z' or 'n' or 'N')
                    {
                        _ = double.TryParse(chars[..^1], NumberStyles.Float, CultureInfo.InvariantCulture, out z);
                    }
                    else if (((!hasWorld && word == 1) || (hasWorld && word == 2)) && chars[^1] is 'e' or 'E')
                    {
                        _ = double.TryParse(chars[..^1], NumberStyles.Float, CultureInfo.InvariantCulture, out x);
                        x = -x;
                    }
                    else if (((!hasWorld && word == 0) || (hasWorld && word == 1)) && chars[^1] is 's' or 'S')
                    {
                        _ = double.TryParse(chars[..^1], NumberStyles.Float, CultureInfo.InvariantCulture, out z);
                        z = -z;
                    }
                    else if (((!hasWorld && word == 2) || (hasWorld && word == 3)) && chars[^1] is 'a' or 'A')
                    {
                        _ = double.TryParse(chars[..^1], NumberStyles.Float, CultureInfo.InvariantCulture, out y);
                    }
                    else if (((!hasWorld && word == 3) || (hasWorld && word == 4)) && double.TryParse(chars,
                                 NumberStyles.Float, CultureInfo.InvariantCulture, out double temp))
                    {
                        yaw = temp;
                    }
                }
            }
        }

        /// <summary>
        ///     Returns a value indicating whether the specified span of characters represents a relative unit string.
        /// </summary>
        /// <param name="bytes">The span of characters to validate.</param>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="bytes" /> represents a valid relative unit string; otherwise,
        ///     <see langword="false" />.
        /// </returns>
        public static bool IsAbsoluteUnit(ReadOnlySpan<byte> bytes)
        {
            Span<char> chars = stackalloc char[bytes.Length];
            Encoding.UTF8.GetChars(bytes, chars);
            return IsRelativeUnit(chars);
        }

        /// <summary>
        ///     Returns a value indicating whether the specified span of characters represents a relative unit string.
        /// </summary>
        /// <param name="chars">The span of characters to validate.</param>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="chars" /> represents a valid relative unit string; otherwise,
        ///     <see langword="false" />.
        /// </returns>
        public static bool IsAbsoluteUnit(ReadOnlySpan<char> chars)
        {
            ReadOnlySpan<char> validChars = "nNeEwWsSaA";
            return double.TryParse(chars, out _) ||
                   (validChars.Contains(chars[^1]) &&
                    double.TryParse(chars[..^1], NumberStyles.Float, CultureInfo.InvariantCulture, out _));
        }

        /// <summary>
        ///     Returns a value indicating whether the specified span of characters represents a relative unit string.
        /// </summary>
        /// <param name="bytes">The span of characters to validate.</param>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="bytes" /> represents a valid relative unit string; otherwise,
        ///     <see langword="false" />.
        /// </returns>
        public static bool IsRelativeUnit(ReadOnlySpan<byte> bytes)
        {
            Span<char> chars = stackalloc char[bytes.Length];
            Encoding.UTF8.GetChars(bytes, chars);
            return IsRelativeUnit(chars);
        }

        /// <summary>
        ///     Returns a value indicating whether the specified span of characters represents a relative unit string.
        /// </summary>
        /// <param name="chars">The span of characters to validate.</param>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="chars" /> represents a valid relative unit string; otherwise,
        ///     <see langword="false" />.
        /// </returns>
        public static bool IsRelativeUnit(ReadOnlySpan<char> chars)
        {
            return (chars[0] == '+' || chars[0] == '-') &&
                   double.TryParse(chars, NumberStyles.Float, CultureInfo.InvariantCulture, out _);
        }

        /// <summary>
        ///     Returns a value indicating whether the specified span of characters represents a valid coordinate unit string.
        /// </summary>
        /// <param name="bytes">The span of characters to validate.</param>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="bytes" /> represents a valid coordinate unit string; otherwise,
        ///     <see langword="false" />.
        /// </returns>
        public static bool IsUnitString(ReadOnlySpan<byte> bytes)
        {
            Span<char> chars = stackalloc char[bytes.Length];
            Encoding.UTF8.GetChars(bytes, chars);
            return IsUnitString(chars);
        }

        /// <summary>
        ///     Returns a value indicating whether the specified span of characters represents a valid coordinate unit string.
        /// </summary>
        /// <param name="chars">The span of characters to validate.</param>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="chars" /> represents a valid coordinate unit string; otherwise,
        ///     <see langword="false" />.
        /// </returns>
        public static bool IsUnitString(ReadOnlySpan<char> chars)
        {
            chars = chars.Trim();

            if (chars.Length == 0)
            {
                return false;
            }

            if (!char.IsDigit(chars[0]) && chars[0] != '+' && chars[0] != '-')
            {
                return false;
            }

            //                              thicc char span
            return IsRelativeUnit(chars) || IsAbsoluteUnit(chars);
        }
    }
}
