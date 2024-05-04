using System.Text.RegularExpressions;

/// <summary>
/// Represents a struct which contains Virtual Paradise coordinates.
/// </summary>
public struct OldCoordinates : IEquatable<OldCoordinates>
{
    /// <summary>
    /// Gets or sets the direction.
    /// </summary>
    public double Direction { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance represents relative coordinates.
    /// </summary>
    public bool IsRelative { get; set; }

    /// <summary>
    /// Gets or sets the world.
    /// </summary>
    public string World { get; set; }

    /// <summary>
    /// Gets or sets the X coordinate.
    /// </summary>
    public double X { get; set; }

    /// <summary>
    /// Gets or sets the Y coordinate.
    /// </summary>
    public double Y { get; set; }

    /// <summary>
    /// Gets or sets the Z coordinate.
    /// </summary>
    public double Z { get; set; }

    public static bool operator ==(OldCoordinates left, OldCoordinates right) =>
        left.Equals(right);

    public static bool operator !=(OldCoordinates left, OldCoordinates right) =>
        !(left == right);

    /// <summary>
    /// Parses a coordinate string.
    /// </summary>
    /// <param name="coordinates">The coordinates to parse.</param>
    /// <returns>Returns an instance of <see cref="OldCoordinates"/>.</returns>
    public static OldCoordinates ParseFaster(string coordinates)
    {
        const string pattern =
            @"(?:([a-z]+) +)?(?: *(\+)?(-?\d+(?:\.\d+)?)([ns])? +(\+)?(-?\d+(?:\.\d+)?)([we])?( +(\+)?(-?\d+(?:\.\d+)?)a)?( +(\+)?(-?\d+(?:\.\d+)?))?)?";
        Regex regex = new(pattern, RegexOptions.IgnoreCase);

        Match match = regex.Match(coordinates);
        bool relative = match.Groups[2].Success || match.Groups[5].Success;

        string world = match.Groups[1].Success ? match.Groups[1].Value : string.Empty;
        double z = match.Groups[3].Success ? double.Parse(match.Groups[3].Value) : 0.0;
        double x = match.Groups[6].Success ? double.Parse(match.Groups[6].Value) : 0.0;
        double y = match.Groups[10].Success ? double.Parse(match.Groups[10].Value) : 0.0;
        double direction = match.Groups[13].Success ? double.Parse(match.Groups[13].Value) : 0.0;

        if (match.Groups[4].Success &&
            match.Groups[4].Value.Equals("S", StringComparison.InvariantCultureIgnoreCase))
        {
            z = -z;
        }

        if (match.Groups[7].Success &&
            match.Groups[7].Value.Equals("E", StringComparison.InvariantCultureIgnoreCase))
        {
            x = -x;
        }

        return new OldCoordinates
        {
            Z = z,
            X = x,
            Y = y,
            Direction = direction,
            World = world,
            IsRelative = relative
        };
    }

    /// <summary>
    /// Parses a coordinate string.
    /// </summary>
    /// <param name="coordinates">The coordinates to parse.</param>
    /// <returns>Returns an instance of <see cref="OldCoordinates"/>.</returns>
    public static OldCoordinates Parse(string coordinates)
    {
        const string pattern =
            @"(?:([a-z]+) *)?(?: *(\+)?(-?\d+(?:\.\d+)?)([ns])? +(\+)?(-?\d+(?:\.\d+)?)([we])?( +(\+)?(-?\d+(?:\.\d+)?)a)?( +(\+)?(-?\d+(?:\.\d+)?))?)?";

        var regex = new Regex(pattern, RegexOptions.IgnoreCase);
        Match match = regex.Match(coordinates);
        bool relative = match.Groups[2].Success || match.Groups[5].Success;

        string world = match.Groups[1].Success ? match.Groups[1].Value : string.Empty;
        double z = match.Groups[3].Success ? Convert.ToDouble(match.Groups[3].Value) : 0.0;
        double x = match.Groups[6].Success ? Convert.ToDouble(match.Groups[6].Value) : 0.0;
        double y = match.Groups[10].Success ? Convert.ToDouble(match.Groups[10].Value) : 0.0;
        double direction = match.Groups[13].Success ? Convert.ToDouble(match.Groups[13].Value) : 0.0;

        if (match.Groups[4].Success &&
            match.Groups[4].Value.Equals("S", StringComparison.InvariantCultureIgnoreCase))
        {
            z = -z;
        }

        if (match.Groups[7].Success &&
            match.Groups[7].Value.Equals("E", StringComparison.InvariantCultureIgnoreCase))
        {
            x = -x;
        }

        return new OldCoordinates
        {
            Z = z,
            X = x,
            Y = y,
            Direction = direction,
            World = world,
            IsRelative = relative
        };
    }

    /// <summary>
    /// Returns the string representation of these coordinates.
    /// </summary>
    /// <returns>Returns a <see cref="String"/>.</returns>
    public override string ToString()
    {
        return ToString("{0}");
    }

    /// <summary>
    /// Returns the string representation of these coordinates.
    /// </summary>
    /// <param name="format">The format to apply to each component.</param>
    /// <returns>Returns a <see cref="String"/>.</returns>
    public string ToString(string format)
    {
        var result = "";

        if (!string.IsNullOrWhiteSpace(World))
        {
            result += $"{World} ";
        }

        bool north = Z >= 0.0;
        bool west = X >= 0.0;
        bool up = Y >= 0.0;
        bool dir = Direction >= 0.0;

        if (IsRelative)
        {
            string zChar = north ? "+" : "";
            string xChar = west ? "+" : "";
            string upChar = up ? "+" : "";
            string dirChar = dir ? "+" : "";

            result += zChar + string.Format(format, Z) + " " +
                      xChar + string.Format(format, X) + " " +
                      upChar + string.Format(format, Y) + " " +
                      dirChar + string.Format(format, Direction);
        }
        else
        {
            string zChar = north ? "n" : "s";
            string xChar = west ? "w" : "e";
            result += zChar + string.Format(format, Z) + " " +
                      xChar + string.Format(format, X) + " " +
                      string.Format(format, Y) + "a " +
                      string.Format(format, Direction);
        }

        return result;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is OldCoordinates other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = Direction.GetHashCode();
            hashCode = (hashCode * 397) ^ X.GetHashCode();
            hashCode = (hashCode * 397) ^ Y.GetHashCode();
            hashCode = (hashCode * 397) ^ Z.GetHashCode();
            return hashCode;
        }
    }

    /// <inheritdoc />
    public bool Equals(OldCoordinates other)
    {
        return Direction.Equals(other.Direction) &&
               X.Equals(other.X) &&
               Y.Equals(other.Y) &&
               Z.Equals(other.Z);
    }
}