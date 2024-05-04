using System.Text;

namespace E012_SourceGeneratorDummy;

/// <summary>
///     Represents a reader that can read a <see cref="StringBuilder" />.
/// </summary>
public class StringBuilderReader : TextReader
{
    private readonly StringBuilder _stringBuilder;
    private int _index;

    /// <summary>
    ///     Initializes a new instance of the <see cref="StringBuilderReader" /> class.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> to wrap.</param>
    /// <exception cref="ArgumentNullException"><paramref name="stringBuilder" /> is <see langword="null" />.</exception>
    public StringBuilderReader(StringBuilder stringBuilder)
    {
        _stringBuilder = stringBuilder ?? throw new ArgumentNullException(nameof(stringBuilder));
    }

    /// <inheritdoc />
    public override int Read()
    {
        if (_index >= _stringBuilder.Length)
            return -1;

        return _stringBuilder[_index++];
    }

    /// <inheritdoc />
    public override int Read(char[] buffer, int index, int count)
    {
        if (buffer is null)
            throw new ArgumentNullException(nameof(buffer));
        if (index < 0)
            throw new ArgumentOutOfRangeException(nameof(index));
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count));
        if (buffer.Length - index < count)
            throw new ArgumentException("The buffer is too small.", nameof(buffer));
        if (_index >= _stringBuilder.Length)
            return -1;

        int length = Math.Min(_stringBuilder.Length - _index, count);
        _stringBuilder.CopyTo(_index, buffer, index, length);
        _index += length;
        return length;
    }

    /// <inheritdoc />
    public override int Read(Span<char> buffer)
    {
        int count = Math.Min(buffer.Length, _stringBuilder.Length - _index);
        for (var index = 0; index < count; index++)
            buffer[index] = _stringBuilder[index + _index];

        _index += count;
        return count;
    }

    /// <inheritdoc />
    public override int ReadBlock(Span<char> buffer)
    {
        return Read(buffer);
    }

    /// <inheritdoc />
    public override int Peek()
    {
        if (_index >= _stringBuilder.Length)
            return -1;

        return _stringBuilder[_index];
    }

    /// <inheritdoc />
    public override int ReadBlock(char[] buffer, int index, int count)
    {
        if (_index >= _stringBuilder.Length)
            return -1;

        int length = Math.Min(count, _stringBuilder.Length - _index);
        _stringBuilder.CopyTo(_index, buffer, index, length);
        _index += length;
        return length;
    }

    /// <inheritdoc />
    public override string? ReadLine()
    {
        if (_index >= _stringBuilder.Length)
            return null;

        int start = _index;
        while (_index < _stringBuilder.Length && _stringBuilder[_index] != '\n')
            _index++;

        if (_index < _stringBuilder.Length)
            _index++;

        return _stringBuilder.ToString(start, _index - start - 1);
    }

    /// <inheritdoc />
    public override string ReadToEnd()
    {
        return _stringBuilder.ToString();
    }

    /// <inheritdoc />
    public override void Close()
    {
        _index = _stringBuilder.Length;
    }
}
