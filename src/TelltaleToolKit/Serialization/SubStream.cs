namespace TelltaleToolKit.Serialization;

/// <summary>
///     Represents a substream of an underlying <see cref="Stream" />.
/// </summary>
public class SubStream : Stream
{
    private readonly long _length;

    private readonly long _offset;

    private readonly Stream _stream;

    private long _position;

    /// <summary>
    ///     Creates a new substream instance using the specified underlying stream at the specified offset with the specified
    ///     length.
    /// </summary>
    /// <param name="stream">The underlying stream.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="length">The length.</param>
    public SubStream(Stream stream, long offset, long length)
    {
        if (stream == null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        // Streams must support seeking for the concept of substreams to work.
        // At a pinch in the future we may support a poor man's seek (forward) by reading until the position is correct.

        if (!stream.CanSeek)
        {
            throw new NotSupportedException("Stream does not support seeking.");
        }

        _stream = stream;

        if (offset < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset cannot be less than zero.");
        }

        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length cannot be less than zero.");
        }

        _offset = offset;
        _length = length;
    }

    /// <inheritdoc />
    public override long Length => _length;

    /// <inheritdoc />
    public override bool CanRead => _stream.CanRead;

    /// <inheritdoc />
    public override bool CanSeek => _stream.CanSeek;

    /// <inheritdoc />
    public override bool CanWrite => _stream.CanWrite;

    /// <inheritdoc />
    public override bool CanTimeout => _stream.CanTimeout;

    /// <inheritdoc />
    public override int ReadTimeout
    {
        get => _stream.ReadTimeout;
        set => throw new NotSupportedException("Cannot set the read timeout of a substream.");
    }

    /// <inheritdoc />
    public override int WriteTimeout
    {
        get => _stream.WriteTimeout;
        set => throw new NotSupportedException("Cannot set the write timeout of a substream.");
    }

    /// <inheritdoc />
    public override long Position
    {
        get => _position;
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("Position cannot be less than zero.");
            }

            if (value > _length)
            {
                throw new ArgumentOutOfRangeException("Position cannot be greater than the length.");
            }

            _stream.Position = _offset + (_position = value);
        }
    }

    /// <inheritdoc />
    public override int Read(byte[] buffer, int offset, int count)
    {
        if (!_stream.CanRead)
        {
            throw new NotSupportedException("Underlying stream does not support reading.");
        }

        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be less than zero.");
        }

        _stream.Seek(_offset + _position, SeekOrigin.Begin);
        _stream.Read(buffer, offset, count = Convert.ToInt32(Math.Min(count, _length - _position)));

        _position += count;

        return count;
    }

    /// <inheritdoc />
    public override void Write(byte[] buffer, int offset, int count)
    {
        if (!_stream.CanWrite)
        {
            throw new NotSupportedException("Underlying stream does not support writing.");
        }

        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be less than zero.");
        }

        _stream.Seek(_offset + _position, SeekOrigin.Begin);
        _stream.Write(buffer, offset, count = Convert.ToInt32(Math.Min(count, _length - _position)));

        _position += count;
    }

    /// <inheritdoc />
    public override long Seek(long offset, SeekOrigin origin)
    {
        switch (origin)
        {
            case SeekOrigin.Begin:
                if (offset < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(offset),
                        "Offset cannot be less than zero when seeking from the beginning.");
                }

                if (offset > _length)
                {
                    throw new ArgumentOutOfRangeException(nameof(offset),
                        "Offset cannot be greater than the length of the substream.");
                }

                _stream.Seek(_offset + (_position = offset), SeekOrigin.Begin);

                break;
            case SeekOrigin.End:
                if (offset > 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(offset),
                        "Offset cannot be greater than zero when seeking from the end.");
                }

                if (offset < -_length)
                {
                    throw new ArgumentOutOfRangeException(nameof(offset),
                        "Offset cannot be less than the length of the substream.");
                }

                _stream.Seek(_position = _length + offset, SeekOrigin.End);

                break;
            case SeekOrigin.Current:
                if (_position + offset < 0)
                {
                    throw new NotSupportedException("Attempted to seek before the start of the substream.");
                }

                if (_position + offset > _length)
                {
                    throw new NotSupportedException("Attempted to seek beyond the end of the substream.");
                }

                _stream.Seek(_position += offset, SeekOrigin.Current);

                break;
        }

        return _position;
    }

    /// <inheritdoc />
    public override void SetLength(long value) => throw
        // While other Stream implementations allow the caller to set the length, this makes little sense in the context of a substream.
        // Perhaps, in the future, we can allow callers to reduce the length but not expand the length.
        new NotSupportedException("Cannot set the length of a fixed substream.");

    /// <inheritdoc />
    public override void Flush() => _stream.Flush();
}
