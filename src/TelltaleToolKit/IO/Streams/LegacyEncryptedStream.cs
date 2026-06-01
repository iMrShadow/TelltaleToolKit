using TelltaleToolKit.Utility.Blowfish;

namespace TelltaleToolKit.IO.Streams;

/// <summary>
///     Stream wrapper that decrypts TTArchive per‑file data on the fly.
///     For simplicity, it reads the entire file into memory – these files are small
///     (config files, scripts) so the overhead is acceptable.
/// </summary>
internal sealed class LegacyEncryptedStream : Stream
{
    private readonly Stream _baseStream;
    private readonly long _headerSize;
    private readonly long _dataLength;
    private readonly int _blockSize;
    private readonly int _cryptInterval; // Blowfish every N blocks
    private readonly int _cleanInterval; // skip XOR every M blocks (if not Blowfish)
    private readonly Blowfish _blowfish;

    private long _position;
    private int _cachedBlockIndex = -1;
    private byte[]? _cachedBlockData;
    private int _cachedBlockValidLength; // for last partial block

    /// <summary>
    /// Initializes a new legacy encrypted stream.
    /// </summary>
    /// <param name="baseStream">Underlying stream (contains the asset header + encrypted data).</param>
    /// <param name="version">MetaStreamVersion that determines the block parameters.</param>
    /// <param name="headerSize">Number of bytes at the beginning of <paramref name="baseStream"/> that form the asset header (e.g. the magic). This header is not decrypted and is skipped.</param>
    /// <param name="blowfish">The Blowfish algorithm used for decrypting.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="baseStream"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="headerSize"/> is negative or larger than the stream length.</exception>
    public LegacyEncryptedStream(Stream baseStream, uint version, long headerSize, Blowfish blowfish)
        : this(baseStream, GetBlockConfiguration(version), headerSize, blowfish)
    {
    }

    private LegacyEncryptedStream(Stream baseStream, (int blockSize, int cryptInterval, int cleanInterval) config,
        long headerSize, Blowfish blowfish)
    {
        _baseStream = baseStream ?? throw new ArgumentNullException(nameof(baseStream));
        if (!_baseStream.CanRead) throw new NotSupportedException("Base stream must be readable.");
        if (!_baseStream.CanSeek) throw new NotSupportedException("Base stream must be seekable.");

        if (headerSize < 0 || headerSize > _baseStream.Length)
            throw new ArgumentException("Invalid header size", nameof(headerSize));

        _headerSize = headerSize;
        _dataLength = _baseStream.Length - headerSize;
        _blockSize = config.blockSize;
        _cryptInterval = config.cryptInterval;
        _cleanInterval = config.cleanInterval;
        _blowfish = blowfish;
        _position = 0;
    }

    /// <inheritdoc />
    public override bool CanRead => true;

    /// <inheritdoc />
    public override bool CanSeek => true;

    /// <inheritdoc />
    public override bool CanWrite => false;

    /// <inheritdoc />
    public override long Length => _dataLength;

    /// <inheritdoc />
    public override long Position
    {
        get => _position;
        set
        {
            if (value < 0 || value > _dataLength)
                throw new ArgumentOutOfRangeException(nameof(value));
            _position = value;
        }
    }

    /// <inheritdoc />
    public override int Read(byte[] buffer, int offset, int count)
        => Read(buffer.AsSpan(offset, count));

    /// <inheritdoc />
    public override int Read(Span<byte> buffer)
    {
        if (buffer.IsEmpty) return 0;
        long remaining = _dataLength - _position;
        if (remaining <= 0) return 0;
        int toRead = (int)Math.Min(buffer.Length, remaining);

        int totalRead = 0;
        while (totalRead < toRead)
        {
            // Determine current block index
            int blockIdx = (int)(_position / _blockSize);
            int blockOffset = (int)(_position % _blockSize);
            int bytesInBlock = (int)Math.Min(_blockSize - blockOffset, _dataLength - _position);

            // Get decrypted block data
            EnsureBlockDecrypted(blockIdx, out byte[] blockData, out int _);

            int copyLen = Math.Min(bytesInBlock, toRead - totalRead);
            blockData.AsSpan(blockOffset, copyLen).CopyTo(buffer[totalRead..]);

            totalRead += copyLen;
            _position += copyLen;
        }

        return totalRead;
    }

    private void EnsureBlockDecrypted(int blockIdx, out byte[] blockData, out int validLength)
    {
        if (_cachedBlockIndex == blockIdx && _cachedBlockData != null)
        {
            blockData = _cachedBlockData;
            validLength = _cachedBlockValidLength;
            return;
        }

        // Read encrypted block from base stream
        long blockStartInBase = _headerSize + (long)blockIdx * _blockSize;
        int blockSizeThis = _blockSize;
        bool isFullBlock = true;
        if (blockStartInBase + _blockSize > _baseStream.Length)
        {
            blockSizeThis = (int)(_baseStream.Length - blockStartInBase);
            isFullBlock = false;
        }

        if (blockSizeThis <= 0)
        {
            blockData = [];
            validLength = 0;
            _cachedBlockIndex = blockIdx;
            _cachedBlockData = blockData;
            _cachedBlockValidLength = 0;
            return;
        }

        _baseStream.Seek(blockStartInBase, SeekOrigin.Begin);
        byte[] encrypted = new byte[blockSizeThis];
        int read = _baseStream.Read(encrypted, 0, blockSizeThis);
        if (read != blockSizeThis)
            throw new EndOfStreamException("Unexpected end of base stream while reading block.");

        // Only apply decryption to FULL blocks (the trailing partial block is plaintext)
        if (isFullBlock)
        {
            if (blockIdx % _cryptInterval == 0)
            {
                _blowfish.Decipher(encrypted.AsSpan(), encrypted.Length);
            }
            else if (blockIdx % _cleanInterval != 0) // not a clean block → XOR
            {
                for (int i = 0; i < encrypted.Length; i++)
                    encrypted[i] = (byte)(encrypted[i] ^ 0xFF);
            }
            // else: clean block – leave as is
        }

        _cachedBlockData = encrypted;
        _cachedBlockValidLength = encrypted.Length;
        _cachedBlockIndex = blockIdx;
        blockData = encrypted;
        validLength = encrypted.Length;
    }

    /// <inheritdoc />
    public override long Seek(long offset, SeekOrigin origin)
    {
        long newPos = origin switch
        {
            SeekOrigin.Begin => offset,
            SeekOrigin.Current => _position + offset,
            SeekOrigin.End => _dataLength + offset,
            _ => throw new ArgumentOutOfRangeException(nameof(origin))
        };
        if (newPos < 0 || newPos > _dataLength)
            throw new ArgumentOutOfRangeException(nameof(offset));
        _position = newPos;
        return _position;
    }

    public override void Flush()
    {
    }

    public override void SetLength(long value) => throw new NotSupportedException();
    public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            // We do not own the base stream; do not dispose it.
            _cachedBlockData = null;
        }

        base.Dispose(disposing);
    }

    // Helper to get block configuration from MetaStreamVersion
    private static (int blockSize, int cryptInterval, int cleanInterval) GetBlockConfiguration(
        uint version)
    {
        return version switch
        {
            1 => (64, 64, 100),
            2 => (128, 32, 80),
            3 => (256, 8, 24),
            _ => throw new ArgumentException($"No legacy encryption scheme defined for version {version}",
                nameof(version))
        };
    }
}
